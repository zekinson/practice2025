using Xunit;
using task04;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldBeStrongerThanFighter()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(cruiser.FirePower > fighter.FirePower);
    }
    // Тесты Cruiser
    // Тесты движения
    [Fact]
    public void CruiserMoveForwardBeforeRotate_RightPosition()
    {
        var cruiser = new Cruiser();
            
        cruiser.MoveForward();
            
        Assert.Equal(50.0, cruiser.X, 10);
        Assert.Equal(0.0, cruiser.Y, 10);
    }

    [Fact]
    public void CruiserMoveForwardAfterRotateLess360_RightPosition()
    {
        var cruiser = new Cruiser();
            
        cruiser.Rotate(90);
        cruiser.MoveForward();
            
        Assert.Equal(0.0, cruiser.X, 10);
        Assert.Equal(50.0, cruiser.Y, 10);
    }
        
    [Fact]
    public void CruiserMoveForwardAfterRotate360_RightPosition()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(360);
        cruiser.MoveForward();
        
        Assert.Equal(50.0, cruiser.X, 10);
        Assert.Equal(0.0, cruiser.Y, 10);
    }

    [Fact]
    public void CruiserMoveForwardAfterRotateMore360_RightPosition()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(450);
        cruiser.MoveForward();
        
        Assert.Equal(0.0, cruiser.X, 10);
        Assert.Equal(50.0, cruiser.Y, 10);
    }

    [Fact]
        public void CruiserMoveForwardAfterRotate45_RightPosition()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(45);
        cruiser.MoveForward();
        
        Assert.Equal(cruiser.X, cruiser.Y, 10);
    }

    // Тесты поворота

    [Fact]
    public void CruiserRotate0_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(0);
        
        Assert.Equal(0, cruiser.Angle);
    }

    [Fact]
    public void CruiserRotate45_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(45);
        
        Assert.Equal(45, cruiser.Angle);
    }

    [Fact]
    public void CruiserRotateNegativeAngle_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(-45);
        
        Assert.Equal(315, cruiser.Angle);
    }

    [Fact]
    public void CruiserRotate360_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(360);
        
        Assert.Equal(0, cruiser.Angle);
    }

    [Fact]
    public void CruiserRotateMoreThan360_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(400);
        
        Assert.Equal(40, cruiser.Angle);
    }

    [Fact]
    public void CruiserRotate_MultipleRotatate_RightAngle()
    {
        var cruiser = new Cruiser();
        
        cruiser.Rotate(100);
        cruiser.Rotate(60);
        
        Assert.Equal(160, cruiser.Angle);
    }

    // Тесты выстрела

    [Fact]
    public void CruiserFire_RightAmmo()
    {
        var cruiser = new Cruiser();

        cruiser.Fire();
        
        Assert.Equal(9, cruiser.Ammo);
    }

    [Fact]
    public void CruiserFireMoreThanAmmo_RightAmmo()
    {
        var cruiser = new Cruiser();
        for (int i = 0; i < 11; i++)
        {
            cruiser.Fire();
        }
        
        Assert.Equal(0, cruiser.Ammo);
    }

    // Тесты Fighter
    // Тесты движения
    [Fact]
    public void FighterMoveForwardBeforeRotate_RightPosition()
    {
        var fighter = new Fighter();
            
        fighter.MoveForward();
            
        Assert.Equal(100.0, fighter.X, 10);
        Assert.Equal(0.0, fighter.Y, 10);
    }

    [Fact]
    public void FighterMoveForwardAfterRotateLess360_RightPosition()
    {
        var fighter = new Fighter();
            
        fighter.Rotate(90);
        fighter.MoveForward();
            
        Assert.Equal(0.0, fighter.X, 10);
        Assert.Equal(100.0, fighter.Y, 10);
    }
        
    [Fact]
    public void FighterMoveForwardAfterRotate360_RightPosition()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(360);
        fighter.MoveForward();
        
        Assert.Equal(100.0, fighter.X, 10);
        Assert.Equal(0.0, fighter.Y, 10);
    }

    [Fact]
    public void FighterMoveForwardAfterRotateMore360_RightPosition()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(450);
        fighter.MoveForward();
        
        Assert.Equal(0.0, fighter.X, 10);
        Assert.Equal(100.0, fighter.Y, 10);
    }

    [Fact]
        public void FighterMoveForwardAfterRotate45_RightPosition()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(45);
        fighter.MoveForward();
        
        Assert.Equal(fighter.X, fighter.Y, 10);
    }

    // Тесты поворота

    [Fact]
    public void FighterRotate0_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(0);
        
        Assert.Equal(0, fighter.Angle);
    }

    [Fact]
    public void FighterRotate45_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(45);
        
        Assert.Equal(45, fighter.Angle);
    }

    [Fact]
    public void FighterRotateNegativeAngle_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(-45);
        
        Assert.Equal(315, fighter.Angle);
    }

    [Fact]
    public void FighterRotate360_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(360);
        
        Assert.Equal(0, fighter.Angle);
    }

    [Fact]
    public void FighterRotateMoreThan360_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(400);
        
        Assert.Equal(40, fighter.Angle);
    }

    [Fact]
    public void FighterRotate_MultipleRotatate_RightAngle()
    {
        var fighter = new Fighter();
        
        fighter.Rotate(100);
        fighter.Rotate(60);
        
        Assert.Equal(160, fighter.Angle);
    }

    // Тесты выстрела

    [Fact]
    public void FighterFire_RightAmmo()
    {
        var fighter = new Fighter();

        fighter.Fire();
        
        Assert.Equal(9, fighter.Ammo);
    }

    [Fact]
    public void FighterFireMoreThanAmmo_RightAmmo()
    {
        var fighter = new Fighter();
        for (int i = 0; i < 11; i++)
        {
            fighter.Fire();
        }
        
        Assert.Equal(0, fighter.Ammo);
    }
}