using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("PlayerVariables")]
    public int jumpCounter;
    public bool canDash;
    public bool canGrapple;

    [Header("Refrences")]
    public Canvas canvas;
    public RawImage progressBar;
    public Controller controller;
    public Image dashButton;
    public Image grappleButton;

    [Header("Hud Textures")]
    public Sprite fullBar;
    public Sprite halfBar;
    public Sprite noBar;
    public Sprite canButton;
    public Sprite cantButton;

    // Update is called once per frame
    void Update()
    {
        UpdateJumpHud();
        UpdateButtons();

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }


    void UpdateJumpHud() {
            if (controller.jumpCounter == 2)
            {
                progressBar.texture = fullBar.texture;
            }
            else if (controller.jumpCounter == 1)
            {
                progressBar.texture = halfBar.texture;
            }
            else
            {
                progressBar.texture = noBar.texture;
            }
        }

    void UpdateButtons() {
        if (controller.currentDashCount > 0)
        {
            dashButton.sprite = canButton;
        }
        else {
            dashButton.sprite = cantButton;
        }

        if (controller.noOfGrapples > 0) {
            grappleButton.sprite = canButton;
        }
        else
        {
            grappleButton.sprite = cantButton;
        }
    
    }
    }

