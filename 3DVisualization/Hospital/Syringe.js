// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a Syringe object in a 3D scene, with a texture and size.
// ============================================================================

import * as THREE from "three";
import { FBXLoader } from 'three/addons/loaders/FBXLoader.js'; // Make sure to use the correct import path

/*
 * parameters = {
 *  modelUrl: String, // URL of the FBX file to be loaded
 *  scale: Number, // Scale factor for the object
 *  rotation: { x: Number, y: Number, z: Number } // Rotation angles in radians for each axis
 * }
 */

export default class Syringe {
    /**
     * Constructor for the Syringe class.
     * 
     * @param {Object} parameters - Configuration object for the Syringe.
     */
    constructor(parameters) {
        // Assign the provided parameters to the instance dynamically
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        
        // Create a Three.js group to hold the Syringe object
        this.object = new THREE.Group();

        // Initialize the FBXLoader for loading the 3D model
        this.fbxLoader = new FBXLoader();

        // Load the 3D model using FBXLoader
        this.fbxLoader.load(
            this.modelUrl, // URL of the FBX file to load
            (object) => {
                // Set the scale of the loaded object
                object.scale.set(0.012, 0.022, 0.0202);
                
                // Add the loaded object to the group
                this.object.add(object);
            },
            undefined,
            (error) => {
                console.error('An error occurred while loading the syringe model:', error);
            }
        );
    }
}