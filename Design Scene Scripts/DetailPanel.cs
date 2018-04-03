using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviour {

    InputField[] InputFields;


	void Start () {
        InputFields = GetComponentsInChildren<InputField>();
	}

    public void ResetInputFields()
    {
        foreach (InputField inputfield in InputFields)
        {
            inputfield.text = "";
        }
    }

    // Input field functions
    public void Name(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        if (CurrentObject != null)
        {
            CurrentObject.name = input;
        }
    }
    public void XPositionInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempPos = CurrentObject.transform.position;
                TempPos.x = value;
                CurrentObject.transform.position = TempPos;
            }
        }
        catch { }
    }
    public void YPositionInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempPos = CurrentObject.transform.position;
                TempPos.z = value;
                CurrentObject.transform.position = TempPos;
            }
        }
        catch { }
    }
    public void ZPositionInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempPos = CurrentObject.transform.position;
                TempPos.y = value;
                CurrentObject.transform.position = TempPos;
            }
        }
        catch { }
    }
    public void XRotationInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempRot = CurrentObject.transform.localEulerAngles;
                TempRot.x = value;
                CurrentObject.transform.localEulerAngles = TempRot;
            }
        }
        catch { }
    }
    public void YRotationInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempRot = CurrentObject.transform.localEulerAngles;
                TempRot.z = value;
                CurrentObject.transform.localEulerAngles = TempRot;
            }
        }
        catch { }
    }
    public void ZRotationInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempRot = CurrentObject.transform.localEulerAngles;
                TempRot.y = value;
                CurrentObject.transform.localEulerAngles = TempRot;
            }
        }
        catch { }
    }
    public void HeightInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempScale = CurrentObject.transform.localScale;
                TempScale.y = value;
                CurrentObject.transform.localScale = TempScale;
                Vector3 TempPos = CurrentObject.transform.position;
                TempPos.y = value / 2;
                CurrentObject.transform.position = TempPos;
            }
        }
        catch { }
    }
    public void WidthInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempScale = CurrentObject.transform.localScale;
                TempScale.x = value;
                CurrentObject.transform.localScale = TempScale;
            }
        }
        catch { }
    }

    public void DoorWidthInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        if (CurrentObject != null)
        {
            GameObject AttachedWall = CurrentObject.GetComponent<Door>().WallAttachedTo;
            if (AttachedWall == null)
            {
                try
                {
                    float value = float.Parse(input);
                    Vector3 TempScale = CurrentObject.transform.localScale;
                    TempScale.x = value;
                    CurrentObject.transform.localScale = TempScale;
                    
                }
                catch { }
            }
            else
            {
                try
                {
                    float value = float.Parse(input);
                    Vector3 TempScale = CurrentObject.transform.localScale;
                    TempScale.x = value/AttachedWall.transform.localScale.x;
                    CurrentObject.transform.localScale = TempScale;
                }
                catch { }
            }
        }
    }

    public void LengthInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Vector3 TempScale = CurrentObject.transform.localScale;
                TempScale.z = value;
                CurrentObject.transform.localScale = TempScale;
            }
        }
        catch { }
    }

    public void DoorHeightInputFieldChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        if (CurrentObject != null)
        {
            GameObject AttachedWall = CurrentObject.GetComponent<Door>().WallAttachedTo;
            if (AttachedWall == null)
            {
                try
                {
                    float value = float.Parse(input);
                    Vector3 TempScale = CurrentObject.transform.localScale;
                    TempScale.y = value;
                    CurrentObject.transform.localScale = TempScale;

                }
                catch { }
            }
            else
            {
                try
                {
                    float value = float.Parse(input);
                    Vector3 TempScale = CurrentObject.transform.localScale;
                    TempScale.y = value / AttachedWall.transform.localScale.y;
                    CurrentObject.transform.localScale = TempScale;
                }
                catch { }
            }
        }
    }
    public void OpacityChange(string input)
    {
        GameObject CurrentObject = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DesignSceneGameManager>().GetTempObjectHolder();
        try
        {
            float value = float.Parse(input);
            if (CurrentObject != null)
            {
                Color TempColor = CurrentObject.GetComponent<Renderer>().material.color;
                TempColor.a = value;
                CurrentObject.GetComponent<Renderer>().material.color = TempColor;
            }
        }
        catch { }
    }
}
