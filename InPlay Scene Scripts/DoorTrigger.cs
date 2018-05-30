using System.Collections;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pedestrian")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            Door doorinfo = this.gameObject.GetComponent<Door>();
            doorinfo.NextScene.SetActive(true);
            GameObject currentscene = other.gameObject.transform.parent.gameObject;
            Vector3 newplayerpos = other.gameObject.transform.position;
            other.gameObject.transform.SetParent(doorinfo.NextScene.transform);
            newplayerpos.x = doorinfo.NextScene.GetComponent<SceneInfo>().PlayerX;
            newplayerpos.y = newplayerpos.y + doorinfo.NextScene.transform.position.y;
            newplayerpos.z = doorinfo.NextScene.GetComponent<SceneInfo>().PlayerY; 
            other.gameObject.transform.localPosition = newplayerpos; 
            currentscene.SetActive(false);
        }
    }
}
