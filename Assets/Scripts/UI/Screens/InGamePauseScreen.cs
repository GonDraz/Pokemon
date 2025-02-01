using GonDraz.UI;

namespace UI.Screens
{
    public class InGamePauseScreen : Presentation
    {
        public void OnBackGameButtonClick()
        {
            GlobalStateMachine.Instance.ChangeState<GlobalStateMachine.InGameState>(false);
        }
    }
}