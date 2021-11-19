using UnityEngine;
using UnityEngine.UI;

public class LogicGridController : MonoBehaviour
{

    [Range(.1f, 10)]
    public float displaySize;

    private Vector3 gridOffset = new Vector3(-.5f, -.5f, 0);

    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Button updateButton;

    private LogicGrid logicGrid;
    // Start is called before the first frame update
    void Start()
    {
        this.logicGrid = new LogicGrid(64, 64);

        LogicComponentFactory factory = new LogicComponentFactory();
        GridComponentPlug[] notAndAndGatePlugs = new GridComponentPlug[2] {
            new GridComponentPlug(0, 0, CardinalDirection.NORTH, ComponentPlugType.OUTPUT, null),
            new GridComponentPlug(0, 0, CardinalDirection.SOUTH, ComponentPlugType.INPUT, null) };

        GridComponentPlug[] twoWideComponents = new GridComponentPlug[3] {
            new GridComponentPlug(0, 0, CardinalDirection.SOUTH, ComponentPlugType.INPUT, null),
            new GridComponentPlug(1, 0, CardinalDirection.SOUTH, ComponentPlugType.INPUT, null),
            new GridComponentPlug(0, 1, CardinalDirection.NORTH, ComponentPlugType.OUTPUT, null)
        };

        GridComponentPlug[] turnPlugs = new GridComponentPlug[2] {
            new GridComponentPlug(0, 0, CardinalDirection.NORTH, ComponentPlugType.OUTPUT, null),
            new GridComponentPlug(0, 0, CardinalDirection.EAST, ComponentPlugType.INPUT, null) };

        factory.RegisterLogicComponent(LogicComponentType.NOT, new Vector2Int(1, 1), new NotGate(), true, notAndAndGatePlugs, null);
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.NOT, 9, 1, CardinalDirection.WEST, false));
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.NOT, 9, 0, CardinalDirection.WEST, false));

        factory.RegisterLogicComponent(LogicComponentType.BUFFER, new Vector2Int(1, 1), new BufferGate(), true, notAndAndGatePlugs, null);

        factory.RegisterLogicComponent(LogicComponentType.AND, new Vector2Int(2, 2), new AndGate(), true, twoWideComponents, null);
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.AND, 5, 0, CardinalDirection.WEST, false));

        factory.RegisterLogicComponent(LogicComponentType.TURN, new Vector2Int(1, 1), null, true, turnPlugs, new UpdateAllOutputPlugs());
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.TURN, 0, 0, CardinalDirection.NORTH, false));

        this.display();


    }

    private void display()
    {
        float xScale = 1f / this.transform.lossyScale.x;
        float yScale = 1f / this.transform.lossyScale.y;
        float zScale = 1f / this.transform.lossyScale.z;
        float renderScale = this.displaySize / this.logicGrid.GetWidth();

        foreach (LogicComponent comp in logicGrid.GetLogicComponents())
        {
            GameObject logicCube = Instantiate(this.cubePrefab, this.transform);
            logicCube.transform.localPosition = getGridPositions(comp.GetBody());
            logicCube.transform.localScale = new Vector3(comp.GetBody().GetArea().width * xScale, comp.GetBody().GetArea().height * yScale, zScale) * renderScale;
            logicCube.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 1, 1);
        }

        foreach (LogicConnection connection in logicGrid.GetLogicConnections())
        {
            GameObject go = Instantiate(this.cubePrefab, this.transform);
            go.transform.localPosition = getGridPositions(connection.GetBody());
            go.transform.localScale = new Vector3(connection.GetBody().GetArea().width * xScale, connection.GetBody().GetArea().height * yScale, zScale);
            if (connection.GetBody().GetDirection() == CardinalDirection.NORTH || connection.GetBody().GetDirection() == CardinalDirection.SOUTH)
            {
                go.transform.localScale = new Vector3(go.transform.localScale.x / 2f, go.transform.localScale.y, .5f);
            }
            else
            {
                go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y / 2f, .5f);
            }

            Vector3 connectionScale = go.transform.localScale;
            connectionScale.z = connectionScale.z * zScale;
            go.transform.localScale = connectionScale * renderScale;

            if (connection.OutputPlug.GetValue() > 0)
            {
                go.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, .2f);
            }
            else
            {
                go.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .2f);
            }
        }
    }

    private Vector3 vector2IntToVector3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    private Vector3 getGridPositions(GridBody  body) {
        float xScale = 1f / this.transform.lossyScale.x;
        float yScale = 1f / this.transform.lossyScale.y;
        float zScale = 1f / this.transform.lossyScale.z;
        float renderScale = this.displaySize / this.logicGrid.GetWidth();
        Vector3 logicPosition = this.vector2IntToVector3(body.GetArea().position);
        logicPosition.z = -.5f;
        logicPosition.x *= xScale;
        logicPosition.y *= yScale;
        logicPosition += new Vector3((body.GetArea().width) / 2f * xScale, (body.GetArea().height) / 2f * yScale, -.5f * renderScale * zScale);
        logicPosition.x *= renderScale;
        logicPosition.y *= renderScale;
        logicPosition += gridOffset;
        return logicPosition;
    }

    public void updateGraph() {
        Debug.Log("clicked");
        updateButton.onClick.AddListener(() =>
        {
            this.logicGrid.UpdateGrid();
            foreach (Transform transform in this.transform)
            {
                GameObject.Destroy(transform.gameObject);
            }
            this.display();
        });
    }
}
