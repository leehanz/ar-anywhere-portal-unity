using UnityEngine;
using UnityEngine.Rendering;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private Material[] places;

    [SerializeField]
    private Renderer environment;

    public Transform device;

    //bool for checking if the device is not in the same direction as it was
    bool facingPortal = false;

    //bool for knowing that on the next change of state, what to set the stencil test
    bool inOtherWorld = false;

    void Start()
    {
        device = Camera.main.transform;
        //start outside other world
        SetMaterials(inOtherWorld = false);
    }

    void OnDestroy()
    {
        SetMaterials(inOtherWorld = false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform != device) return;
        facingPortal = isUserInFrontOfPortal();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform != device) return;

        // Is user currently facing portal proceeding after collision enter.
        bool isInFront = isUserInFrontOfPortal();
        Debug.Log(isInFront);
        if ((isInFront && !facingPortal) || (facingPortal && !isInFront))
        {
            // Toggle state of base reality.
            SetMaterials(inOtherWorld = !inOtherWorld);
        }
        // Update based on collision state whether or not
        // user is facing portal or moved away.
        facingPortal = isInFront;
    }

    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
        Shader.SetGlobalInt("_StencilTest", (int)stencilTest);
    }

    bool isUserInFrontOfPortal()
    {
        Vector3 tempPosition = transform.InverseTransformPoint(device.position);
        return (tempPosition.z >= 0) ? true : false;
    }

    public void SwitchPlace(int index)
    {
        environment.material = places[index];
    }
}
