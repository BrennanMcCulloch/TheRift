using UnityEngine;
using DialogueTree;

    public class PrerequisiteError : System.Exception {
        public PrerequisiteError(string message) : base(message) { }
    }

    public abstract class Prerequisite : MonoBehaviour {
        public abstract bool Met();
    }