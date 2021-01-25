using UnityEngine;

namespace io.spectar.UniVDOM.Tests.Fixtures
{
    public class TodoItemPresenter : MonoBehaviour
    {
        public string text;

        private TextMesh textMesh;

        public string Text
        {
            get => text;
            set
            {
                text = value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // textMesh = GetComponentInChildren<TextMesh>();
        }

        // Update is called once per frame
        void Update()
        {
            // textMesh.text = text;
        }
    }
}
