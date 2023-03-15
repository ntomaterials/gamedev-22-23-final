using UnityEngine;
using UnityEngine.UI;
public class DialogWindow : MonoBehaviour
{
public static DialogWindow Instance;
public Text nameText;
public Text textArea;
private void Awake()
{
    Instance=this;
}
}
