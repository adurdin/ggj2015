﻿using UnityEngine;
using System.Collections;

public class CannonTowerSegment : TowerSegment {

	public AudioClip cannonClip;

	public override float OnGetConstructionDuration() {
		return GameConstants.CANNONS_TOWER_SEGMENT_BUILD_TIME;
	}

	public override float OnGetActionDuration() {
		return GameConstants.CANNONS_TOWER_SEGMENT_ACTION_TIME;
	}
	
	public override int OnGetTribeCost() {
		return GameConstants.CANNONS_TOWER_SEGMENT_TRIBE_COST;
	}

	public override bool OnIsActionable () {
		return true;
	}
	
	public override bool OnIsComplete () {
		return true;
	}
	
	public override void OnProgressAction (float secondsRemaining) {
		// FIXME - animate cannon fire?
	}
	
	public override void OnCompleteAction () {
		m_owningTower.DestroyOpponentsSegment(this);
		AudioSource.PlayClipAtPoint(cannonClip, Vector3.zero);
		this.Reset ();
	}
}
