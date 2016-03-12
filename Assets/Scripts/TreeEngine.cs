using UnityEngine;
using System.Collections;

public class TreeEngine : MonoBehaviour {

	private GameObject broadLeafTree;
	private GameObject coniferTree;

	void Awake() {
		broadLeafTree = Resources.Load ("TreePrefabs/BroadLeafTree") as GameObject;
		coniferTree = Resources.Load ("TreePrefabs/ConiferTree") as GameObject;
	}

	// Use this for initialization
	void Start () {
		generateTreeTest ();
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}

	void generateTreeTest() {
		for (int x=0; x<=100; x+=10) {
			for(int z=0; z<=100; z+=10) {
				int occurs = Random.Range(0,4);
				if(occurs == 0) {
					int treeType = Random.Range(0,2);
					if(treeType == 0) {
						GameObject bTree = (GameObject)Instantiate(broadLeafTree, new Vector3(x,0.0f,z), Quaternion.identity);
					}
					else if(treeType == 1) {
						GameObject cTree = (GameObject)Instantiate(coniferTree, new Vector3(x,0.0f,z), Quaternion.identity);
					}
				}
			}
		}
	}
}