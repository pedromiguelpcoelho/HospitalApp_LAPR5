// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: Class to manage and apply fog effect in the scene
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  enabled: Boolean,
 *  color: Integer,
 *  near: Float,
 *  far: Float
 * }
 */

export default class Fog {
    constructor(parameters) {
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Create the fog
        this.object = new THREE.Fog(this.color, this.near, this.far);
    }
}