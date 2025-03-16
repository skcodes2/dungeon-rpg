using UnityEngine;
[System.Serializable]
public class SkillsTreeButton
{
    private bool isAbilityUpgrade;
    private bool isActive;
    private int price;
    private string name;
    private SkillsTreeButton nextButton;
    private bool isPurchased;


    public SkillsTreeButton(bool isAbilityUpgrade, bool isActive, int price, string name, SkillsTreeButton nextButton)
    {
        this.isAbilityUpgrade = isAbilityUpgrade;
        this.isActive = isActive;
        this.price = price;
        this.name = name;
        this.nextButton = nextButton;
        this.isPurchased = false;

    }

    // Getters
    public bool getIsAbilityUpgrade()
    {
        return this.isAbilityUpgrade;
    }

    public bool getIsActive()
    {
        return this.isActive;
    }

    public int getPrice()
    {
        return this.price;
    }

    public string getName()
    {
        return this.name;
    }

    public SkillsTreeButton getNextButton()
    {
        return this.nextButton;
    }

    public bool getIsPurchased()
    {
        return this.isPurchased;
    }



    // Setters

    public void setIsAbilityUpgrade(bool isAbilityUpgrade)
    {
        this.isAbilityUpgrade = isAbilityUpgrade;
    }

    public void setIsActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void setPrice(int price)
    {
        this.price = price;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public void setNextButton(SkillsTreeButton nextButton)
    {
        this.nextButton = nextButton;
    }

    public void setIsPurchased(bool isPurchased)
    {
        this.isPurchased = isPurchased;
    }



    public void purchase()
    {
        this.isPurchased = true;
    }


}