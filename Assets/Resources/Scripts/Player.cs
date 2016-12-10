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


    private TSTransform TSTransform;
    private TSRigidBody TSRigidBody;

    private TSVector lastTapInput = TSVector.zero;


    /**
    * @brief It is true if the ball is not dynamically instantiated.
    **/
    public bool createdRuntime;

    [Tooltip("Gameobject prefab to be used as a placeholder for a line")]
    [SerializeField]
    private GameObject _linePrefab;

    [Tooltip("Prefab for Player characters")]
    [SerializeField]
    private GameObject _characterPrefab;

    [Tooltip("Gameobject prefab to be used as a position marker for where the player clicks")]
    [SerializeField]
    private GameObject _positionMarkerPrefab;

    [Tooltip("Distance from center of tap point to location of force applied to reactive objects")]
    [SerializeField]
    private FP _tapEffectRadius = new FP(3);

    [Tooltip("Amount of force applied to object near tapping")]
    [SerializeField]
    private FP _tapEffectForce = new FP(5);

    [Tooltip("Players spawn this distance from center")]
    [SerializeField]
    private float _playerSpawnDistanceFromCenter = 7.5f;

    [Tooltip("Materials used to display local player's highlight area")]
    [SerializeField]
    private Material _localPlayerHighlightMaterial;

    [Tooltip("Material used to display all other player's highlight areas")]
    [SerializeField]
    private Material _otherPlayerHighlightMaterial;

    public Mesh _areaHighlightMesh;
    public MeshCollider _areaHighlightMeshCollider;
    public MeshRenderer _areaHighlightMeshRenderer;
    public MeshFilter _areaHighlightMeshFilter;


    private static LayerMask _lineLayerMask;	// Used for checking for line collisions, lines are on this layer.
    private static LayerMask _areaHighlightlayerMask;
    private static List<Player> _playerList = new List<Player>();  // list of players in game

    private static Dictionary<GameObject, Player> _areaHighlightMeshGOToPlayerDict = new Dictionary<GameObject, Player>();
    private static Dictionary<int, Player> _photonPlayerIDToPlayerDict = new Dictionary<int, Player>();
    private static float[] _intermediateBoundaryAngles = { 45f, 135f, 225f, 315f }; // Angles representing the corners on the screen
    private static float _distanceFromCenterToScreenEdge = 50f;

    public GameObject backgroundGO;

    private TSTransform _cameraTSTransform;


    private static List<TSRigidBody> _lineList = new List<TSRigidBody>();
    private Transform _line1;   // 1st line assigned to this player
    private Transform _line2;   // 2nd line assigned to this player


    private static void AdjustLineAssignments()
    {
        for(int index = 0; index < _playerList.Count; index++)
        {
            if (index == 0)
            {
                _playerList[0]._line1 = _lineList[0].transform;
                _playerList[0]._line2 = _lineList[1].transform;
            }
            else if (index == 1)
            {
                _playerList[1]._line1 = _lineList[1].transform;
                _playerList[1]._line2 = _lineList[0].transform;
            }
            else
            {
                _playerList[index]._line1 = _lineList[_lineList.Count - 1].transform;
                _playerList[index]._line2 = _lineList[0].transform;
                _playerList[index - 1]._line2 = _lineList[_lineList.Count - 1].transform;
            }
        }
        
    }


    void Awake()
    {
        
        
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
            rb.MovePosition(new TSVector(0,1,0));
            //rb.angularVelocity = TSVector.up;
            rb.tsTransform.rotation = TSQuaternion.AngleAxis(index * lineAngle, TSVector.up);
            rb.gameObject.name = "Line " + index;
        }
    }

    private static Material[] _areaHighlightMaterials = new Material[4];

    void Start()
    {
        _playerList.Add(this);

        _lineLayerMask = LayerMask.GetMask("AreaBoundaryLine");
        _areaHighlightlayerMask = LayerMask.GetMask("PlayerArea");
        _areaHighlightMesh = new Mesh();
        _areaHighlightMeshCollider = gameObject.AddComponent<MeshCollider>();
        _areaHighlightMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        _areaHighlightMeshFilter = gameObject.AddComponent<MeshFilter>();

        _areaHighlightMaterials[0] = Resources.Load("Materials/blue", typeof(Material)) as Material;
        _areaHighlightMaterials[1] = Resources.Load("Materials/green", typeof(Material)) as Material;
        _areaHighlightMaterials[2] = Resources.Load("Materials/red", typeof(Material)) as Material;
        _areaHighlightMaterials[3] = Resources.Load("Materials/yellow", typeof(Material)) as Material;

        _areaHighlightMeshRenderer.material = Resources.Load("Materials/yellow", typeof(Material)) as Material;

        _cameraTSTransform = Camera.main.GetComponent<TSTransform>();
        
        TSRigidBody = GetComponent<TSRigidBody>();

        if (_playerList.Count == 1)
        {
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());
        }
        else if (_playerList.Count > 2)
            _lineList.Add(TrueSyncManager.SyncedInstantiate(Resources.Load("Prefabs/prefab_Line") as GameObject, TSVector.up, TSQuaternion.identity).GetComponent<TSRigidBody>());

        AdjustLineAssignments();
        RepositionLines();
        HighlightPlayerAreas();
    }

    public Player GetPlayerWithPhotonPlayerID(int ID)
    {
        return _photonPlayerIDToPlayerDict[ID];
    }


    private static void HighlightPlayerAreas()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            Player player = _playerList[i];
            player._areaHighlightMeshRenderer.material = _areaHighlightMaterials[i];
            List <int> trianglesList = new List<int>();
            List<Vector3> vertexList = new List<Vector3>();
            vertexList.Add(Vector3.zero); // create the 1st vertex in the center
            trianglesList.Add(0); // Add the 1st vertex to the list

            player._areaHighlightMesh.Clear(); // erase the previous mesh
            player._areaHighlightMesh.subMeshCount = 1; // reset the submesh count
            float[] highlightAngles = GetPlayerAreaHighlightAnglesBetweenLines(player._line1, player._line2);
            Vector3 vec = Quaternion.Euler(0f, player._line1.transform.eulerAngles.y + 90f, 0f) * Vector3.left * _distanceFromCenterToScreenEdge;
            vertexList.Add(vec);
            trianglesList.Add(vertexList.Count - 1);

            foreach (float angle in highlightAngles) // Add any intermediate faces between line1 and line2
            {
                vertexList.Add(Quaternion.Euler(0f, angle, 0f) * Vector3.left * _distanceFromCenterToScreenEdge);
                trianglesList.Add(vertexList.Count - 1);
                trianglesList.Add(0);
                trianglesList.Add(vertexList.Count - 1);
            }
            vertexList.Add(Quaternion.Euler(0f, player._line2.transform.eulerAngles.y + 90f, 0f) * Vector3.left * _distanceFromCenterToScreenEdge);
            trianglesList.Add(vertexList.Count - 1);

            Vector2[] uvs = new Vector2[player._areaHighlightMesh.vertices.Length];
            for (int v = 0; v < uvs.Length; v++)
                uvs[v] = Vector2.up;

            player._areaHighlightMesh.vertices = vertexList.ToArray();
            player._areaHighlightMesh.triangles = trianglesList.ToArray();
            player._areaHighlightMesh.uv = uvs;

            player._areaHighlightMesh.RecalculateNormals();
            player._areaHighlightMesh.Optimize();
            player._areaHighlightMesh.RecalculateBounds();

            player._areaHighlightMeshFilter.sharedMesh = null;
            player._areaHighlightMeshFilter.sharedMesh = player._areaHighlightMesh;
            //_areaHighlightMeshCollider.sharedMesh = null;
            //_areaHighlightMeshCollider.sharedMesh = _areaHighlightMesh;


            for (int j = 0; j <= trianglesList.Count - 1; j++)
            {
                if (j < trianglesList.Count - 1)
                    Debug.DrawLine(vertexList[trianglesList[j]], vertexList[trianglesList[j + 1]], Color.magenta);
            }

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

        TSTransform = this.GetComponent<TSTransform>();
        
        // if is first player then changes ball's color to black
        if (owner != null && owner.Id == 1)
        {
            //GetComponent<Renderer>().material.color = Color.black;
        }

        if (!createdRuntime)
        {

        }
    }


    /**
    * @brief Sets player inputs.
    **/
    public override void OnSyncedInput()
    {
        if (createdRuntime) return;
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, _areaHighlightlayerMask))
            lastTapInput = hit.point.ToTSVector();
        else
            lastTapInput = TSVector.zero;

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

        TSVector forceToApply = TSVector.zero;
        TSTransform tst = GetComponent<TSTransform>();

        if (tapLocation != TSVector.zero)
        {
            //Debug.Log("tl: "+tapLocation+" tst: "+transform.position);
        }


        // Display the forward vectors of each line in cyan color:
        foreach (TSRigidBody rb in _lineList)
        {
            Debug.DrawRay(rb.tsTransform.position.ToVector(), rb.tsTransform.forward.ToVector() * 10, Color.cyan, 0.05f);
        }

        HighlightPlayerAreas();

        //controlledBody.AddForce(forceToApply, ForceMode.Impulse);
        //lastCreateState = currentCreateState;
    }


    /// <summary>
    /// OnTapLocation - call this to handle distributing and processing taps across a synchronized network game session
    /// Responds to taps and decides what to do
    /// </summary>
    /// <param name="tapLocation">location where tap/click took place</param>
    private void OnTapLocation(TSVector tapLocation)
    {

        // -get angle of this tap, compare to each line's angle
        // -find the closest line
        // if the closest line is close enough for a tap effect, apply the tap effect

        FP tapAngle = TSVector.up.AngleBetween(TSVector.forward, tapLocation);
        TSRigidBody closestLine = _lineList[0];
        //FP closestResult = new FP(360);
        FP closestResult = new FP(-1);

        foreach (TSRigidBody thisLine in _lineList)
        {

            FP result = TSVector.Dot(thisLine.tsTransform.forward, tapLocation.normalized);

            if (result > closestResult)
            {
                closestResult = result;
                closestLine = thisLine;
            }
        }

        // We have the closest line, determine if it's close enough for effect
        //TSVector pointOfForce = closestLine.tsTransform.forward * tapLocation.magnitude;
        FP p = TSVector.Dot(closestLine.tsTransform.forward, tapLocation.normalized);

        // Point on line nearest tap contact is made
        TSVector linePoint = closestLine.tsTransform.forward * tapLocation.magnitude;

        if (_tapEffectRadius >= TSVector.Distance(linePoint, tapLocation))
        {
            FP direction = TSVector.Cross(tapLocation.normalized, closestLine.tsTransform.forward).y;
            closestLine.AddTorque(TSVector.up * direction * _tapEffectForce);
        }

        /*
        if (Physics.Raycast(Camera.main.ScreenPointToRay(tapLocation), out hit, 50f, _areaHighlightlayerMask))
        {
            TSRigidBody.AddForce(((transform.position - hit.point).normalized * 100).ToTSVector());
            PhysicsManager.instance.
            /*
            if (!Physics.CheckSphere(hit.point, _tapRadiusCheck, _lineLayerMask))
            {
                if (_areaHighlightMeshGOToPlayerDict.ContainsKey(hit.collider.gameObject))
                {
                    Line closestLine = _areaHighlightMeshGOToPlayerDict[hit.collider.gameObject].GetClosestLine(hit.point);
                    float clickAng = new Vector2(hit.point.z, hit.point.x).GetAngle();
                    // FIXME: Get line angle here:
                    //float lineAng = (float)closestLine.TrueAngle.ToDouble();
                    //double dir = (Mathf.Abs(clickAng - lineAng) > 180 ? 1 : -1) * (clickAng > lineAng ? 1 : -1);
                    // FIXME: Set line's angular velocity here:
                    //closestLine.AngularVelocity += FNum.Create(dir * _tapEffectForce / 50);
                }

                // Shows where click took place:
                GameObject positionMarker = GameObject.Instantiate(_positionMarkerPrefab);
                positionMarker.transform.position = hit.point;
                positionMarker.name = "Position Marker";    // Identify this marker game object in unity editor
                Destroy(positionMarker.GetComponent<SphereCollider>(), 1f);
                Renderer rend = positionMarker.GetComponent<Renderer>();
                DOTween.ToAlpha(() => rend.material.color, x => rend.material.color = x, 0, 5f).OnComplete(() =>
                {
                    Destroy(positionMarker);
                });
            }
            */
    }
    }


/*
    public PhotonPlayer PhotonPlayer { get; set; }
    public GameObject CharacterGO { get; set; }
    public Line Line1 { get; set; }
    public Line Line2 { get; set; }
    public GameObject AreaHighlightMeshGO { get; set; }
    public Mesh AreaHighlightMesh { get; set; }
    public Material AreaHighlighMeshMaterial { get; set; }
    public MeshFilter AreaHighlightMeshFilter { get; set; }
    public MeshRenderer AreaHighlightMeshRenderer { get; set; }
    public MeshCollider AreaHighLightMeshCollider { get; set; }

    public int num;

    public TrueSync.FP AngleDiff { get; private set; }

    private static int playerNum = 0;



    public Player(int photonPlayerID)
    {
        this.PhotonPlayer = PhotonPlayer.Find(photonPlayerID);
        this.AreaHighlightMeshGO = new GameObject("AreaHighlightGO");
        this.AreaHighlightMeshGO.layer = LayerMask.NameToLayer("PlayerArea");
        this.AreaHighlightMeshFilter = AreaHighlightMeshGO.AddComponent<MeshFilter>();
        this.AreaHighlightMeshFilter.mesh = new Mesh();
        this.AreaHighlightMeshRenderer = AreaHighlightMeshGO.AddComponent<MeshRenderer>();
        this.AreaHighlightMeshRenderer.material = AreaHighlighMeshMaterial;
        this.AreaHighlightMesh = AreaHighlightMeshFilter.mesh;
        this.AreaHighLightMeshCollider = AreaHighlightMeshGO.AddComponent<MeshCollider>();
        this.AreaHighLightMeshCollider.sharedMesh = AreaHighlightMesh;
        num = playerNum++;
    }
    */
/*
    // FIXME: Make this method deterministic
    public Line GetClosestLine(Vector3 point)
    {
    
        return Vector3.Distance(Line1.child.position, point) < Vector3.Distance(Line2.child.position, point) ? Line1 : Line2;
    }
    */

//  public void UpdatePosition(float distanceFromCenter)
//  {
/*
    TrueSync.FP L1angle = Line1.TSTransform.eulerAngles.y;
    TrueSync.FP L2angle = Line2.TSTransform.eulerAngles.y;
    if (L2angle < L1angle)
        L2angle += Line.Limit360;

    // not used in this method, but used to measure angle space for when player loses
    // (I don't like side effects, but this a decent solution since it's related to position)
    AngleDiff = L2angle - L1angle;

    TrueSync.FP posAngle = (L1angle + L2angle) / 2;
    if (posAngle > Line.Limit360)
        posAngle -= Line.Limit360;

    CharacterGO.transform.position = Quaternion.AngleAxis(posAngle.AsFloat(), Vector3.up) * Vector3.forward * distanceFromCenter;
    */
//    }


/*
public Player(int photonPlayerID, int characterPhotonViewID, GameObject line1, GameObject line2, Material material)
{

    this.Line1 = line1;
    this.Line2 = line2;
    this.CharacterGO = PhotonView.Find(characterPhotonViewID).gameObject;
    this.AreaHighlightMeshGO = new GameObject("AreaHighlightGO");
    this.AreaHighlightMeshGO.layer = LayerMask.NameToLayer("PlayerArea");
    this.AreaHighlightMeshFilter = AreaHighlightMeshGO.AddComponent<MeshFilter>();
    this.AreaHighlightMeshGO.AddComponent<MeshRenderer>().material = material;
    this.AreaHighlighMeshMaterial = material;
    this.AreaHighlightMesh = new Mesh();
    this.AreaHighlightMesh.vertices = new Vector3[5];

    AreaHighlightMeshFilter.mesh = AreaHighlightMesh;

    num = playerNum++;
}
*/


public static class ExtensionMethods
{
    public static float GetAngle(this Vector2 vector2)
    {
        float result = (Mathf.Atan2(vector2.y, vector2.x) + Mathf.PI * 2) * Mathf.Rad2Deg;
        if (result > 360)
            result -= 360;
        else if (result < 0)
            result += 360;
        return result;
    }

    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static FP AngleBetween(this TSVector n, TSVector v1, TSVector v2)
    {
        return TSMath.Atan2(
            TSVector.Dot(n, TSVector.Cross(v1, v2)),
            TSVector.Dot(v1, v2)) * TSMath.Rad2Deg;
    }

}