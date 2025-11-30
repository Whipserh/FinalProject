using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(PlayerControls))]
public class PlayerControlStatEditor : Editor
{

    public VisualTreeAsset VisualTreeAsset;


    private PlayerControls playerControls;
    private Button fireCanonBtn;

    private void OnEnable()
    {
        playerControls = (PlayerControls)target;
        //tagerts knows which one to look at thanks to the
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();


        //add all the stuff in the visaul tree to the root
        VisualTreeAsset.CloneTree(root);


        //return base.CreateInspectorGUI(); // base is the deafult drawing
        fireCanonBtn = root.Q<Button>("FireCanonButton");
        fireCanonBtn.RegisterCallback<ClickEvent>(OnCanonFireButtonClick);

        return root;//return what we created
    }

    #region Buttons events
    private void OnCanonFireButtonClick(ClickEvent evt)
    {
        playerControls.fireCanonBall();
    }

    #endregion

}
