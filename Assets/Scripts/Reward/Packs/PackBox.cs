using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameMesh;
    [SerializeField] private Image iconImage;
    [SerializeField] private Color weaponColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private GameObject packLight;
    [SerializeField] private List<ComponentBox> componentBoxes;
    [SerializeField] private List<ProgramBox> programBoxes;
    private Pack storagedPack;

    private void Start()
    {
        componentBoxes.ForEach(x => x.Clear());
    }

    public void ConfigureBox(Pack pack)
    {
        storagedPack = pack;
        storagedPack.Initiate();
        nameMesh.text = storagedPack.name;
        iconImage.sprite = storagedPack.icon;
        iconImage.GetComponent<Animator>().SetInteger("Index", storagedPack.index);

        int i = 0;
        foreach(ComponentBox box in componentBoxes)
        {
            if (pack.rewards.Count == i) break;
            box.ReceiveComponent(pack.rewards[i]);
            i++;
        }

        i = 0;
        programBoxes.ForEach(x => x.gameObject.SetActive(false));

        foreach(ProgramBox programBox in programBoxes)
        {
            if (pack.programs.Count == i) break;
            if (pack.programs[i] != null)
            {
                programBox.gameObject.SetActive(true);
                programBox.SetupFilledBox(pack.programs[i]);
            }
            else programBox.EmptyBox();
            i++;
        }

        foreach(UIAnimations animation in GetComponents<UIAnimations>())
        {
            animation.Play();
        }

        packLight.SetActive(true);
    }

    public void Disable()
    {
        foreach(UIAnimations animation in GetComponents<UIAnimations>())
        {
            animation.PlayReverse();
        }

        iconImage.GetComponent<Animator>().SetTrigger("Reset");

        componentBoxes.ForEach(x => x.Clear());
        programBoxes.ForEach(x => x.EmptyBox());


        packLight.SetActive(false);

    }

    public void SelectPack()
    {
        RewardCalculator.Main.ReceiveRewards(storagedPack.rewards);
        storagedPack.programs.ForEach(x => TurretConstructor.Main.AddUnlockedProgram(x));
        PackOfferManager.Main.RemovePack(storagedPack);
    }
}
