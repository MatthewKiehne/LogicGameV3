using UnityEngine;

public class LogicGridVerticalMover : MonoBehaviour
{
    public float Maxheight = .15f;
    public float MinHeight = -.15f;
    public float MoveSpeed = .125f;

    public void moveUp() {
        float verticalHeight = this.transform.localPosition.y + (Time.deltaTime * MoveSpeed);
        verticalHeight = Mathf.Clamp(verticalHeight, this.MinHeight, this.Maxheight);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, verticalHeight, this.transform.localPosition.z);
    }

    public void moveDown() {
        float verticalHeight = this.transform.localPosition.y - (Time.deltaTime * MoveSpeed);
        verticalHeight = Mathf.Clamp(verticalHeight, this.MinHeight, this.Maxheight);
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, verticalHeight, this.transform.localPosition.z);
    }
}
