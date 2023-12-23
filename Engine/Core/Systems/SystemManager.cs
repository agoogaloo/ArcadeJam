using Engine.Core.Input;
using Microsoft.Xna.Framework.Input;
using ArcadeJam;

namespace Engine.Core.Systems;
public class SystemManager {


    public static SystemManager Instance { get; private set;}

    public RenderSystem RenderSystem  {get;}
    public PhysicsSystem PhysicsSystem  {get;}

    public static void Start(){
        Instance = new SystemManager();
    }
    private SystemManager() {
        
        this.RenderSystem = new RenderSystem();
        PhysicsSystem = new ();
        
    }




}
