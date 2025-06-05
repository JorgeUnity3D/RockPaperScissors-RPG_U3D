using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kapibara.Util.Coroutines
{
    public class CoroutineQueue {
        MonoBehaviour m_Owner = null;
        Coroutine m_InternalCoroutine = null;
        Queue<IEnumerator> actions = new Queue<IEnumerator>();

        IEnumerator currentAction;

        public CoroutineQueue(MonoBehaviour aCoroutineOwner) {
            m_Owner = aCoroutineOwner;
        }

        public void StartLoop() {
            m_InternalCoroutine = m_Owner.StartCoroutine(Process());
        }

        public void StopLoop() {
            m_Owner.StopCoroutine(m_InternalCoroutine);
            m_InternalCoroutine = null;
        }

        public void EnqueueAction(IEnumerator aAction) {
            actions.Enqueue(aAction);
        }

        private IEnumerator Process() {
            while (true) {
                if (actions.Count > 0) {
                    IEnumerator curr = actions.Dequeue();
                    currentAction = curr;
                    yield return m_Owner.StartCoroutine(curr);
                } else {
                    yield return null;
                }
            }
        }

        public void EnqueueWait(float aWaitTime) {
            actions.Enqueue(Wait(aWaitTime));
        }

        private IEnumerator Wait(float aWaitTime) {
            yield return new WaitForSeconds(aWaitTime);
        }

        public IEnumerator GetCurrentAction() {
            return currentAction;
        }
    }
}