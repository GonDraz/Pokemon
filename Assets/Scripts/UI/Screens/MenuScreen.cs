using GonDraz.UI;

namespace UI.Screens
{
    public class MenuScreen : Presentation
    {
        
        public void OnPlayButtonClick(){
            GlobalStateMachine.Instance.ChangeState<GlobalStateMachine.InGameState>();
        }
    }
}