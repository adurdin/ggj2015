﻿using UnityEngine;
using System.Collections;

public class EmptyTowerSegment : TowerSegment {

	private TowerSegment m_towerSegmentToBeConstructed;

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual bool OnIsActionable () {
		return true;
	}
	
	public virtual void OnBeginAction (Tribe tribe) {
		
		/* TODO: Allow player to select new tower segment */
		m_towerSegmentToBeConstructed = m_staticTowerSegmentPrefab;
		
		this.SetNewTowerSegment(m_constructionTowerSegmentPrefab);
	}
	
	public virtual void OnCompleteAction () {
	
	}
}
