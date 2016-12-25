using UnityEngine;
using System.Collections.Generic;
using TrueSync;
using DG.Tweening;


public class Player : TrueSyncBehaviour
{
    /**
    * @brief Key to set/get horizontal position from {@link TrueSyncInput}.
    **/
    private const byte INPUT_TAP_LOCATION = 0;

    public Mesh _areaHighlightMesh;
    public MeshCollider _areaHighlightMeshCollider;
    public MeshRenderer _areaHighlightMeshRenderer;
    public MeshFilter _areaHighlightMeshFilter;

    [SerializeField]
    private Transform _line1;   // 1st line assigned to this player

    [SerializeField]
    private Transform _line2;   // 2nd line assigned to this player

    private Transform _characterTS;

    private static LayerMask _areaHighlightlayerMask;
    private static List<Player> _playerList = new List<Player>();  // list of players in game
    private static float[] _intermediateBoundaryAngles = { 45f, 135f, 225f, 315f }; // Angles representing the corners on the screen
    private static GameObject PositionMarkerGO;
    private static List<TSRigidBody> _lineList = new List<TSRigidBody>();

    private TSVector lastValidTapLocation = TSVector.one * 1000000;


    private static void AdjustLineAssignments()
    {
        int playerCount = _playerList.Count;
        for (int i = playerCount - 1; i >= 0; i--)
        {
            if (i == playerCount - 1 && playerCount > 1) // last player gets a special assignment for its line2, unless there's only 1 player
                _playerList[i]._line2 = _lineList[0].transform;
            else
                _playerList[i]._line2 = _lineList[i+1].transform;

            _playerList[i]._line1 = _lineList[i].transform;
        }
    }


    /// <summary>
    /// RepositionLines - repositions the player lines evenly with a calculated angle determined by the number of players
    /// </summary>
    private void RepositionLines()
    {
        FP lineAngle = FP.Zero;
        if (_playerList.Count < 3)
            lineAngle = new FP(170);
        else
            lineAngle = new FP(360 / _playerList.Count);

        for (int index = 0; index < _lineList.Count; index++)
        {
            TSRigidBody rb = _lineList[index];
            rb.MovePosition(new TSVector(0,5,0));
            //rb.angularVelocity = TSVector.up;
            rb.tsTransform.rotation = TSQuaternion.AngleAxis(index * lineAngle, TSVector.up);
            rb.gameObject.name = "Line " + index;
        }
    }

    void Start()
    {
        _playerList.Add(this);
        this.name = "player " + _playerList.Count;
        _characterTS = GameObject.Instantiate(PlayerConfig.Instance.CharacterPrefab).transform;

        _characterTS.name = "character player " + _playerList.Count;

        _areaHighlightlayerMask = LayerMask.GetMask("PlayerArea");
        _areaHighlightMesh = new Mesh();
        _areaHighlightMeshCollider = gameObject.AddComponent<MeshCollider>();
        _areaHighlightMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        _areaHighlightMeshFilter = gameObject.AddComponent<MeshFilter>();
        
        if (_playerList.Count == 1)
        {
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());
        }
        else if (_playerList.Count > 2)
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());

        if (PositionMarkerGO == null)
            PositionMarkerGO = Resources.Load("Prefabs/prefab_PositionMarker") as GameObject;

        AdjustLineAssignments();
        RepositionLines();
    }
    

    private static void HighlightPlayerAreas()
    {
        float distanceFromCenterToScreenEdge = PlayerConfig.Instance.DistanceFromCenterToScreenEdge;
        for (int i = 0; i < _playerList.Count; i++)
        {
            Player player = _playerList[i];
            if (player.localOwner.Id == player.owner.Id)
            {
                player._areaHighlightMeshRenderer.material = PlayerConfig.Instance.LocalPlayerAreaMaterial;
            }
            else
            {
                player._areaHighlightMeshRenderer.material = PlayerConfig.Instance.RemotePlayerAreaMaterial;
            }

            List <int> trianglesList = new List<int>();
            List<Vector3> vertexList = new List<Vector3>();
            vertexList.Add(Vector3.zero); // create the 1st vertex in the center
            trianglesList.Add(0); // Add the 1st vertex to the list

            player._areaHighlightMesh.Clear(); // erase the previous mesh
            player._areaHighlightMesh.subMeshCount = 1; // reset the submesh count
            float[] highlightAngles = GetPlayerAreaHighlightAnglesBetweenLines(player._line1, player._line2);
            Vector3 vec = Quaternion.Euler(0f, player._line1.transform.eulerAngles.y + 90f, 0f) * Vector3.left * distanceFromCenterToScreenEdge;
            vertexList.Add(vec);
            trianglesList.Add(vertexList.Count - 1);

            foreach (float angle in highlightAngles) // Add any intermediate faces between line1 and line2
            {
                vertexList.Add(Quaternion.Euler(0f, angle, 0f) * Vector3.left * distanceFromCenterToScreenEdge);
                trianglesList.Add(vertexList.Count - 1);
                trianglesList.Add(0);
                trianglesList.Add(vertexList.Count - 1);
            }
            vertexList.Add(Quaternion.Euler(0f, player._line2.transform.eulerAngles.y + 90f, 0f) * Vector3.left * distanceFromCenterToScreenEdge);
            trianglesList.Add(vertexList.Count - 1);

            Vector2[] uvs = new Vector2[player._areaHighlightMesh.vertices.Length];
            for (int v = 0; v < uvs.Length; v++)
                uvs[v] = Vector2.up;

            player._areaHighlightMesh.vertices = vertexList.ToArray();
            player._areaHighlightMesh.triangles = trianglesList.ToArray();
            player._areaHighlightMesh.uv = uvs;

            Vector3[] normals = new Vector3[player._areaHighlightMesh.vertexCount];
            for (int n = 0; n < normals.Length; n++) normals[n] = Vector3.up;
            player._areaHighlightMesh.normals = normals;
            player._areaHighlightMesh.RecalculateBounds();

            player._areaHighlightMeshFilter.sharedMesh = null;
            player._areaHighlightMeshFilter.sharedMesh = player._areaHighlightMesh;
            player._areaHighlightMeshCollider.sharedMesh = null;
            player._areaHighlightMeshCollider.sharedMesh = player._areaHighlightMesh;
        }
    }

    /// <summary>
    /// Calculates the angles between the specified lines to make sure the corners in the highlighted area is filled out
    /// </summary>
    /// <param name="line1"></param>
    /// <param name="line2"></param>
    /// <returns></returns>
    private static float[] GetPlayerAreaHighlightAnglesBetweenLines(Transform line1, Transform line2)
    {
        float angleAdjustment = line1.transform.eulerAngles.y + 90f;
        float angleOfFirstLine = 0f; // make angle of 1st line always zero, as an offset
        float angleOfSecondLine = line2.transform.eulerAngles.y + 90f - angleAdjustment;
        if (angleOfFirstLine > angleOfSecondLine) angleOfSecondLine += 360f;
        float[] tempAngles = new float[4];
        for (int i = 0; i < 4; i++)
        {
            tempAngles[i] = _intermediateBoundaryAngles[i] - angleAdjustment;
            while (tempAngles[i] < 0f)
                tempAngles[i] += 360f;
        }
        List<float> boundaryAnglesFoundBetweenLines = new List<float>();
        for (int j = 0; j < tempAngles.Length; j++)
        {
            if (tempAngles[j] >= angleOfFirstLine && tempAngles[j] <= angleOfSecondLine)
            {
                boundaryAnglesFoundBetweenLines.Add(tempAngles[j]);
            }
        }
        boundaryAnglesFoundBetweenLines.Sort();

        for (int i = 0; i < boundaryAnglesFoundBetweenLines.Count; i++)
        {
            boundaryAnglesFoundBetweenLines[i] += angleAdjustment;
            if (boundaryAnglesFoundBetweenLines[i] > 360)
                boundaryAnglesFoundBetweenLines[i] -= 360f;
        }

        return boundaryAnglesFoundBetweenLines.ToArray();
    }


    /**
    * @brief Initial setup when game is started.
    **/
    public override void OnSyncedStart()
    {
        // Adds {@link #lastJumpState} to the tracking system
        StateTracker.AddTracking(this);
        
        // if is first player then changes ball's color to black
        if (owner != null && owner.Id == 1)
        {
            //GetComponent<Renderer>().material.color = Color.black;
        }
    }


    /**
    * @brief Sets player inputs.
    **/
    public override void OnSyncedInput()
    {
        TSVector lastTapInput = TSVector.zero;
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f))
        {
            if (hit.collider.gameObject == gameObject) // Only register tap if tapping on local player's area
            {
                lastTapInput = hit.point.ToTSVector();
            }
        }

        TrueSyncInput.SetTSVector(INPUT_TAP_LOCATION, lastTapInput);
    }


    /**
    * @brief Updates ball's movements and instantiates new ball objects when player press space.
    **/
    public override void OnSyncedUpdate()
    {
        TSVector tapLocation = TrueSyncInput.GetTSVector(INPUT_TAP_LOCATION);
        
        // Take tapLocation and calculate what to do
        if (tapLocation != TSVector.zero)
            OnTapLocation(tapLocation);

        UpdatePlayerPosition();
    }


    public static void AfterUpdate()
    {
        ApplyDragOnLines();
        HighlightPlayerAreas();
    }

    private void UpdatePlayerPosition()
    {
        Vector3 dir = (_line1.forward + _line2.forward).normalized; // Find direction between lines

        // Calculate which side of the player area, if negative then player needs to be on other side
        float dot = Vector3.Dot(Vector3.up, Vector3.Cross(_line1.forward, _line2.forward));
        dot = (dot > 0) ? 1 : -1;

        _characterTS.position = dir * PlayerConfig.Instance.CharacterDistanceFromCenter * dot;
        _characterTS.LookAt(new Vector3(0, 0, 0));
    }

    private static void ApplyDragOnLines()
    {
        for (int index = 0; index < _lineList.Count; index++)
        {
            _lineList[index].angularVelocity = _lineList[index].angularVelocity.normalized * PlayerConfig.Instance.LineFriction.GetVelocity(_lineList[index].angularVelocity.magnitude);
        }
    }

    private void EffectNearestLine(TSVector tapLocation)
    {
        TSRigidBody closestLine = _lineList[0];
        FP closestResult = new FP(-1); // DOT product of -1, farthest possible value to check direction of a vector

        foreach (TSRigidBody thisLine in _lineList)
        {
            FP result = TSVector.Dot(thisLine.tsTransform.forward, tapLocation.normalized);

            if (result > closestResult)
            {
                closestResult = result;
                closestLine = thisLine;
            }
        }

        if (closestLine.angularVelocity.magnitude > 0) // exit this method if line is moving, can't hit lines while they move
            return;

        // We have the closest line, determine if it's close enough for effect
        // Point on line nearest tap contact is made
        TSVector linePoint = closestLine.tsTransform.forward * tapLocation.magnitude;
        FP distanceToLine = TSVector.Distance(linePoint, tapLocation);
        FP tapEffectRadius = PlayerConfig.Instance.TapEffectRadius;
        FP minTapDistance = PlayerConfig.Instance.MinTapDistance;
        FP direction = TSVector.Cross(tapLocation.normalized, closestLine.tsTransform.forward).normalized.y;
        FP effect = FP.One;

        if (distanceToLine < minTapDistance) // if tapped on line, penalty
        {
            closestLine.AddTorque(TSVector.down * direction * PlayerConfig.Instance.PenaltyEffectForce);
        }
        else if (tapEffectRadius >= distanceToLine) // if tapped within range, but not on line
        {
            effect = 1 - (distanceToLine - minTapDistance) / tapEffectRadius;
            closestLine.AddTorque(TSVector.up * direction * PlayerConfig.Instance.TapEffectForce * effect);
        }
    }

    /// <summary>
    /// OnTapLocation - call this to handle distributing and processing taps across a synchronized network game session
    /// Responds to taps and decides what to do
    /// </summary>
    /// <param name="tapLocation">location where tap/click took place</param>
    private void OnTapLocation(TSVector tapLocation)
    {
        // Shows where click took place (position marker):
        GameObject positionMarker = TrueSyncManager.SyncedInstantiate(PositionMarkerGO, tapLocation, TSQuaternion.identity);
        positionMarker.transform.position = tapLocation.ToVector();
        positionMarker.name = "Position Marker";    // Identify this marker game object in unity editor
        
        TSTransform tst = positionMarker.GetComponent<TSTransform>();
        tst.scale = TSVector.one * PlayerConfig.Instance.MinTapDistance;
        Renderer rend = positionMarker.GetComponent<Renderer>();

        if (TSVector.Distance(lastValidTapLocation, tapLocation) >= PlayerConfig.Instance.MinTapDistanceFromPrevious)
        {
            EffectNearestLine(tapLocation);
            rend.material.color = Color.black;
        }
        else
        {
            rend.material.color = Color.red;
        }

        DOTween.ToAlpha(() => rend.material.color, x => rend.material.color = x, 0, 5f).OnComplete(() =>
        {
            TrueSyncManager.Destroy(positionMarker);
        });

        lastValidTapLocation = tapLocation;
    }
}
