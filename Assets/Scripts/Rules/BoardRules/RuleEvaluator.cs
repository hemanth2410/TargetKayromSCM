using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RuleEvaluator : MonoBehaviour
{
    List<IBoardRule> rules;

    [SerializeField] Color m_ValidColor;
    [SerializeField] Color m_InvalidColor;
    [SerializeField] LayerMask m_LayerMask;
    [SerializeField] bool m_Valid;
    public Color ValidColor { get { return m_ValidColor; } }
    public Color InvalidColor { get { return m_InvalidColor; } }

    GameObject striker;
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.RegisterRuleEvaluator(this);
        rules = GetComponents<IBoardRule>().ToList();
        striker = GameObject.FindGameObjectWithTag(Constants.Tag_Striker);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EvaluateRules()
    {
        m_Valid = rules.All(x => x.ExecuteRule(striker.transform.position, GameController.Instance.GameManager.StrikerForceDirection, CoinType.Faction1, m_LayerMask));
    }
}
