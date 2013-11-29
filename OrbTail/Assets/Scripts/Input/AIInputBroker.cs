using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AIInputBroker: IInputBroker
{
    /// <summary>
    /// Defines an intelligence level for the AI
    /// </summary>
    public enum AIIntelligence{

        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh

    }

    /// <summary>
    /// The intelligence of this AI
    /// </summary>
    public AIIntelligence Intelligence{get; set;}

    public AIInputBroker(AIIntelligence intelligence)
    {

        Intelligence = intelligence;

    }

    public float Acceleration { get; private set; }

    public float Steering { get; private set; }
    
    public ICollection<IGroup> FiredPowerUps
    {
        get { throw new NotImplementedException(); }
    }

    public void Update()
    {
        throw new NotImplementedException();
    }


}