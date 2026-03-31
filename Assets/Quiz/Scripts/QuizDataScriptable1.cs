using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "QuestionData")]
public class QuizDataScriptable1 : ScriptableObject
{
    public string categoryName;
    public List<Question> questions;

}
