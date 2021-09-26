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
        factory.RegisterLogicComponent(LogicComponentType.NOT, new Vector2Int(1, 1), new NotGate(), true, notAndAndGatePlugs, null);

        LogicComponent notGate = factory.CreateLogicComponent(LogicComponentType.NOT, 1, 1, CardinalDirection.NORTH, false);
        this.logicGrid.AddComponent(notGate);
        this.display();

        updateButton.onClick.AddListener(() =>
        {
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
            GameObject logicCube = Instantiate(this.cubePrefab, this.vector2IntToVector3(comp.GetBody().GetArea().position), Quaternion.Euler(0, 0, 0), this.transform);
            logicCube.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 1, 1);
        }

        foreach (LogicConnection connection in logicGrid.GetLogicConnections())
        {
            GameObject logicCube = Instantiate(this.cubePrefab, this.vector2IntToVector3(connection.GetBody().GetArea().position), Quaternion.Euler(0, 0, 0), this.transform);
            logicCube.transform.localScale = new Vector3(connection.GetBody().GetArea().width, connection.GetBody().GetArea().height, 1);
            if (connection.OutputPlug.GetValue() > 0)
            {
                logicCube.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, .2f);
            }
            else
            {
                logicCube.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .2f);
            }

            logicCube.transform.position += new Vector3((connection.GetBody().GetArea().width - 1) / 2f, (connection.GetBody().GetArea().height - 1) / 2f, 0);

            if (connection.GetBody().GetDirection() == CardinalDirection.NORTH || connection.GetBody().GetDirection() == CardinalDirection.SOUTH)
            {
                logicCube.transform.localScale = new Vector3(.5f, logicCube.transform.localScale.y, .5f);
            }
            else
            {
                logicCube.transform.localScale = new Vector3(logicCube.transform.localScale.x, .5f, .5f);
            }
        }
    }

    private Vector3 vector2IntToVector3(Vector2Int vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
}
