using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour {

    public GameObject RelatedPanel;

    public GameObject LinkedGameObject = null;

    // Original information.
    private string OriginalName = null;
    private float x_pos = 0;
    private float y_pos = 0;
    private float z_pos = 0;
    private float x_rot = 0;
    private float y_rot = 0;
    private float z_rot = 0;
    private float length = 1; // associated with y
    private float width = 1; // associated with x
    private float height = 1; // associated with z
    private float opacity = 1;

    // This part is used to store some extra original information of Door object;
    private string WallAttachedTo = null;
    private string NextScene = null;
    private float RelativePosition = 0;
    private float AbsWidth = 0;
    private float AbsHeight = 0;
    private bool IsOpen = false;

    // This part is used to store some extra original information of Fire object;
    private float HRRPUA;
    private float CO_YIELD;
    private float SOOT_YIELD;
    private string FUEL;

    public void SetOriginalInfo(string name = null, float xp = 0, float yp = 0, float zp = 0,
        float xr = 0, float yr = 0, float zr = 0, float l = 1, float w = 1, float h = 1, 
        float o = 1)
    {
        OriginalName = name;
        x_pos = xp; y_pos = yp; z_pos = zp;
        x_rot = xr; y_rot = yr; z_rot = zr;
        length = l; width = w; height = h;
        opacity = o;
    }
    public GameObject GetLinkedGameObject()
    {
        return LinkedGameObject;
    }

    public void SetLink(GameObject gameobject)
    {
        LinkedGameObject = gameobject;
    }

    public void ResetLink()
    {
        LinkedGameObject = null;
    }

    public void Click()
    {
        GameObject gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        if (LinkedGameObject != null & RelatedPanel != null)
        {
            gamemanager.GetComponent<DesignSceneGameManager>().SetTempObjectHolder(LinkedGameObject);
            gamemanager.GetComponent<DesignSceneGameManager>().SetIsExistingObject(true);
            gamemanager.GetComponent<DesignSceneGameManager>().SetLastClickedButton(this.gameObject);
            OriginalName = LinkedGameObject.name;
            x_pos = LinkedGameObject.transform.position.x;
            y_pos = LinkedGameObject.transform.position.z;
            z_pos = LinkedGameObject.transform.position.y;
            x_rot = LinkedGameObject.transform.localEulerAngles.x;
            y_rot = LinkedGameObject.transform.localEulerAngles.z;
            z_rot = LinkedGameObject.transform.localEulerAngles.y;
            width = LinkedGameObject.transform.localScale.x;
            length = LinkedGameObject.transform.localScale.z;
            height = LinkedGameObject.transform.localScale.y;
            opacity = LinkedGameObject.GetComponent<Renderer>().material.color.a;

            // Store some variables for door object
            if (LinkedGameObject.tag == "Door")
            {
                GameObject wallAttachedTo = LinkedGameObject.GetComponent<Door>().WallAttachedTo;
                WallAttachedTo = wallAttachedTo.name;
                NextScene = LinkedGameObject.GetComponent<Door>().NextScene.name;
                RelativePosition = wallAttachedTo.transform.localScale.x * LinkedGameObject.transform.localPosition.x;
                AbsWidth = wallAttachedTo.transform.localScale.x * LinkedGameObject.transform.localScale.x;
                AbsHeight = wallAttachedTo.transform.localScale.y * LinkedGameObject.transform.localScale.y;
                IsOpen = LinkedGameObject.GetComponent<Door>().Open;
            }

            // Store some variables for fire object
            if (LinkedGameObject.tag == "Fire")
            {
                HRRPUA = LinkedGameObject.GetComponent<Fire>().HRRPUA;
                CO_YIELD = LinkedGameObject.GetComponent<Fire>().CO_YIELD;
                SOOT_YIELD = LinkedGameObject.GetComponent<Fire>().SOOT_YIELD;
                FUEL = LinkedGameObject.GetComponent<Fire>().FUEL;
            }

            if (LinkedGameObject.tag == "Wall" || LinkedGameObject.tag == "Floor" ||
                LinkedGameObject.tag == "Ceiling" || LinkedGameObject.tag == "Obstacle"
               )
            {
                InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
                foreach (InputField inputfield in AllInputFields)
                {
                    if (inputfield.name == "Name")
                    {
                        inputfield.text = OriginalName;
                    }
                    else if (inputfield.name == "Wall")
                    {
                        inputfield.text = WallAttachedTo;
                    }
                    else if (inputfield.name == "Next Scene")
                    {
                        inputfield.text = NextScene;
                    }
                    else if (inputfield.name == "x position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.x.ToString();
                    }
                    else if (inputfield.name == "y position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.z.ToString();
                    }
                    else if (inputfield.name == "z position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.y.ToString();
                    }
                    else if (inputfield.name == "z rotation")
                    {
                        inputfield.text = LinkedGameObject.transform.localEulerAngles.y.ToString();
                    }
                    else if (inputfield.name == "Width")
                    {
                        inputfield.text = width.ToString();
                    }
                    else if (inputfield.name == "Length")
                    {
                        inputfield.text = width.ToString();
                    }
                    else if (inputfield.name == "Height")
                    {
                        inputfield.text = height.ToString();
                    }
                    else if (inputfield.name == "Opacity")
                    {
                        inputfield.text = opacity.ToString();
                    }
                }
            }
            else if (LinkedGameObject.tag == "Door")
            {
                InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
                foreach (InputField inputfield in AllInputFields)
                {
                    if (inputfield.name == "Name")
                    {
                        inputfield.text = OriginalName;
                    }
                    else if (inputfield.name == "Wall")
                    {
                        inputfield.text = WallAttachedTo;
                    }
                    else if (inputfield.name == "Next Scene")
                    {
                        inputfield.text = NextScene;
                    }
                    else if (inputfield.name == "Height")
                    {
                        inputfield.text = AbsHeight.ToString();
                    }
                    else if (inputfield.name == "Width")
                    {
                        inputfield.text = AbsWidth.ToString();
                    }
                    else if (inputfield.name == "Relative Position")
                    {
                        inputfield.text = RelativePosition.ToString();
                    }
                }
                Toggle OpenToggle = RelatedPanel.GetComponentInChildren<Toggle>();
                OpenToggle.isOn = IsOpen;
            }
            else if (LinkedGameObject.tag == "Fire")
            {
                InputField[] AllInputFields = RelatedPanel.GetComponentsInChildren<InputField>();
                foreach (InputField inputfield in AllInputFields)
                {
                    if (inputfield.name == "Name")
                    {
                        inputfield.text = OriginalName;
                    }
                    else if (inputfield.name == "x position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.x.ToString();
                    }
                    else if (inputfield.name == "y position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.z.ToString();
                    }
                    else if (inputfield.name == "z position")
                    {
                        inputfield.text = LinkedGameObject.transform.position.y.ToString();
                    }
                    else if (inputfield.name == "Width")
                    {
                        inputfield.text = width.ToString();
                    }
                    else if (inputfield.name == "Length")
                    {
                        inputfield.text = width.ToString();
                    }
                    else if (inputfield.name == "HRRPUA")
                    {
                        inputfield.text = HRRPUA.ToString();
                    }
                    else if (inputfield.name == "CO YIELD")
                    {
                        inputfield.text = CO_YIELD.ToString();
                    }
                    else if (inputfield.name == "SOOT YIELD")
                    {
                        inputfield.text = SOOT_YIELD.ToString();
                    }
                }
                Dropdown Fuel = RelatedPanel.GetComponentInChildren<Dropdown>();
                for (int i = 0; i < Fuel.options.Count; i++)
                {
                    if (Fuel.options[i].text == FUEL)
                    {
                        Fuel.value = i;
                        break;
                    }
                }
            }
        }
    }

    public void Debuger()
    {
        if (LinkedGameObject == null)
        {
            Debug.Log("NoLinkedGameObject");
        }
        else
        {
            Debug.Log("YesLinkedGameObject");
        }
    }

    public void ResetInfo()
    {
        if (LinkedGameObject != null)
        {
            LinkedGameObject.name = OriginalName;
            LinkedGameObject.transform.position = new Vector3(x_pos, z_pos, y_pos);
            LinkedGameObject.transform.localEulerAngles = new Vector3(x_rot, z_rot, y_rot);
            LinkedGameObject.transform.localScale = new Vector3(width, height, length);
        }
    }
}
