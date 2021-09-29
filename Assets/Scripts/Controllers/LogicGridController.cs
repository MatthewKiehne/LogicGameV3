using UnityEngine;
using UnityEngine.UI;

public class LogicGridController : MonoBehaviour
{

    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Button updateButton;

    private LogicGrid logicGrid;
    // Start is called before the first frame update
    void Start()
    {
        this.logicGrid = new LogicGrid(10, 10);

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
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.NOT, 8, 6, CardinalDirection.WEST, false));
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.NOT, 8, 5, CardinalDirection.WEST, false));

        factory.RegisterLogicComponent(LogicComponentType.BUFFER, new Vector2Int(1, 1), new BufferGate(), true, notAndAndGatePlugs, null);
        // this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.BUFFER, 5, 8, CardinalDirection.NORTH, false));
        // this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.BUFFER, 2, 1, CardinalDirection.WEST, false));

        factory.RegisterLogicComponent(LogicComponentType.AND, new Vector2Int(2, 2), new AndGate(), true, twoWideComponents, null);
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.AND, 5, 5, CardinalDirection.WEST, false));

        factory.RegisterLogicComponent(LogicComponentType.TURN, new Vector2Int(1, 1), null, true, turnPlugs, new UpdateAllOutputPlugs());
        this.logicGrid.AddComponent(factory.CreateLogicComponent(LogicComponentType.TURN, 2, 5, CardinalDirection.NORTH, false));

        this.display();

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

    private void display()
    {
        foreach (LogicComponent comp in logicGrid.GetLogicComponents())
        {
            GameObject logicCube = Instantiate(this.cubePrefab, this.transform);
            Vector3 logicPosition = this.vector2IntToVector3(comp.GetBody().GetArea().position);
            logicPosition += new Vector3((comp.GetBody().GetArea().width - 1) / 2f, (comp.GetBody().GetArea().height - 1) / 2f, 0);
            logicCube.transform.position = logicPosition;
            logicCube.transform.localScale = new Vector3(comp.GetBody().GetArea().width, comp.GetBody().GetArea().height, 1);
            logicCube.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 1, 1);
        }

        foreach (LogicConnection connection in logicGrid.GetLogicConnections())
        {
            GameObject go = Instantiate(this.cubePrefab, this.transform);
            go.transform.position = this.vector2IntToVector3(connection.GetBody().GetArea().position);
            go.transform.localScale = new Vector3(connection.GetBody().GetArea().width, connection.GetBody().GetArea().height, 1);
            if (connection.OutputPlug.GetValue() > 0)
            {
                go.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, .2f);
            }
            else
            {
                go.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .2f);
            }

            go.transform.position += new Vector3((connection.GetBody().GetArea().width - 1) / 2f, (connection.GetBody().GetArea().height - 1) / 2f, 0);

            if (connection.GetBody().GetDirection() == CardinalDirection.NORTH || connection.GetBody().GetDirection() == CardinalDirection.SOUTH)
            {
                go.transform.localScale = new Vector3(.5f, go.transform.localScale.y, .5f);
            }
            else
            {
                go.transform.localScale = new Vector3(go.transform.localScale.x, .5f, .5f);
            }
        }
    }

    private Vector3 vector2IntToVector3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
