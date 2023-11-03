using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropCreation : MonoBehaviour
{
    [SerializeField] private PropFactory propFactory;

    [SerializeField] private Button rockBtn;
    [SerializeField] private Button bushBtn;
    [SerializeField] private Button treeBtn;
    
    public Transform spawn;

    private void Awake()
    {
        rockBtn.onClick.AddListener(() =>
        {
            propFactory.CreateProp("Rock", spawn);
        });

        bushBtn.onClick.AddListener(() =>
        {
            propFactory.CreateProp("Bush", spawn);
        });

        treeBtn.onClick.AddListener(() =>
        {
            propFactory.CreateProp("Tree", spawn);
        });
    }
}
