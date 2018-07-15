using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHandler : MonoBehaviour
{
    [SerializeField] Animator m_animator = null;
    [SerializeField] [Range(0.0f, 10.0f)] float m_comboSpeed = 0.25f;
    
    public bool Finished { get; private set; }

    List<ComboPiece> m_combos;
    Queue<ComboPiece> m_comboOrder;
    float m_comboTime;
    int m_currentCombo;

    private void Start()
    {
        m_combos = new List<ComboPiece>() { new NormalAttack(m_animator), new BLTRAttack(m_animator), new TRLAttack(m_animator) };
        m_comboOrder = new Queue<ComboPiece>();
        m_comboTime = 0.0f;
        m_currentCombo = 0;
        Finished = false;
    }

    private void Update()
    {
        m_comboTime += Time.deltaTime;
        
        if (m_comboTime >= m_comboSpeed)
        {
            if (m_currentCombo == 3)
            {
                Finished = true;
            }
            else if (m_comboOrder.Count > 0)
            {
                m_comboOrder.Dequeue().Trigger();
                m_comboTime = 0.0f;
            }
            else
            {
                m_currentCombo = 0;
                m_comboTime = 0.0f;
            }
        }
    }

    public void Attack()
    {
        if (m_currentCombo < 3)
            m_comboOrder.Enqueue(m_combos[m_currentCombo++]);
    }

    public void Reset()
    {
        Finished = false;
        m_comboTime = 0.0f;
        m_currentCombo = 0;
    }

    private abstract class ComboPiece
    {
        protected Animator m_animator;

        public ComboPiece(Animator animator)
        {
            m_animator = animator;
        }

        public abstract void Trigger();
    }

    private class NormalAttack : ComboPiece
    {
        public NormalAttack(Animator animator) : base(animator)
        {

        }

        public override void Trigger()
        {
            m_animator.SetTrigger("Attack_Normal");
        }
    }

    private class BLTRAttack : ComboPiece
    {
        public BLTRAttack(Animator animator) : base(animator)
        {

        }

        public override void Trigger()
        {
            m_animator.SetTrigger("Attack_BL-TR");
        }
    }

    private class TRLAttack : ComboPiece
    {
        public TRLAttack(Animator animator) : base(animator)
        {

        }

        public override void Trigger()
        {
            m_animator.SetTrigger("Attack_TR-L");
        }
    }
}
