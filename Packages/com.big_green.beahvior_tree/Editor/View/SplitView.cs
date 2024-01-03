//----------------------------------------
//author: BigGreen
//date: 2023-12-17 17:25
//----------------------------------------

using UnityEngine.UIElements;

namespace BG.BTEditor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits>
        {
        }
    }
}