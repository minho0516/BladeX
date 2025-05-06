using Swift_Blade.Level.Door;
using UnityEngine;

namespace Swift_Blade
{
    public class Memos : MonoBehaviour, IInteractable
    {
        [SerializeField] private string m_talker;
        [SerializeField] private string m_scripts;
        [SerializeField] private string m_secondScripts;

        [SerializeField] private DialogueDataSO dialogueSO;

        private DialogueDataSO m_dialogueData;

        [SerializeField] private Door doorTrm;
        [SerializeField] private ParticleSystem[] fireworks;

        private void Awake()
        {
            if(m_dialogueData == null) m_dialogueData = new DialogueDataSO();

            TalkingData talkingData = new TalkingData();
            talkingData.talker = m_talker;
            talkingData.dialogueMessage = m_scripts;
            m_dialogueData.dialougueDatas.Clear();
            m_dialogueData.dialougueDatas.Add(talkingData);
        }
        public void Interact()
        {
            if (DialogueManager.Instance.IsDialogueOpen) return;

            DialogueManager.Instance.StartDialogue(dialogueSO);

            /*
            void SecondDialogue()
            {
                if (m_secondScripts == "" && m_secondScripts is null) return;
                Debug.Log("µÎ¹ø¤Š");

                TalkingData talkingData = new TalkingData();
                talkingData.talker = m_talker;
                talkingData.dialogueMessage = m_secondScripts;
                m_dialogueData.dialougueDatas.Clear();
                m_dialogueData.dialougueDatas.Add(talkingData);

                DialogueManager.Instance.StartDialogue(m_dialogueData);
            }*/
            if (doorTrm is not null)
            {
                doorTrm.UpDoor();

                foreach(var firework in fireworks)
                {
                    firework.Play();
                }
            }
        }
    }
}
