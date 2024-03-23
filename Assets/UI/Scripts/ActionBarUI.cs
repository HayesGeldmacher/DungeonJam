using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarUI : MonoBehaviour
{
    public CombatManager CombatManager;
    public int TurnCount = 5;
    public RectTransform Line;
    public RectTransform TickMarkPrefab;
    public ActionPreviewUI ActionPreviewPrefab;
    public float TurnWidth { get { return Line.rect.width / TurnCount; } }

    private List<RectTransform> _tickMarks = new List<RectTransform>();

    private void Start()
    {
        CombatManager = FindObjectOfType<CombatManager>();
        for (int i = 0; i < TurnCount; i++)
        {
            _tickMarks.Add(Instantiate(TickMarkPrefab, Line));
        }
        foreach (var agent in CombatManager.Agents)
        {
            var preview = Instantiate(ActionPreviewPrefab, transform);
            preview.SetCombatAgent(agent);
        }
    }

    private void Update()
    {
        float normalizedTime = CombatManager.NormalizedTurnTime;
        for (int i = 0; i < _tickMarks.Count; i++)
        {
            _tickMarks[i].anchoredPosition = new Vector2((i + normalizedTime) * TurnWidth, 0);
        }
    }

}
