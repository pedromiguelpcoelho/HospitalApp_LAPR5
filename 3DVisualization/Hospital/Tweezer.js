// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a Tweezer object in a 3D scene, with a texture and size.
// ============================================================================

import * as THREE from "three";
import { OBJLoader } from 'three/addons/loaders/OBJLoader.js'; // Make sure to use the correct import path

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture to be applied to the object
 *  scale: Number, // Scale factor for the object
 *  rotation: { x: Number, y: Number, z: Number } // Rotation angles in radians for each axis
 * }
 */

export default class Tweezer {
    /**
     * Constructor for the Tweezer class.
     * 
     * @param {Object} parameters - Configuration object for the Tweezer.
     */
    constructor(parameters) {
        // Assign the provided parameters to the instance dynamically
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        
        // Create a Three.js group to hold the Tweezer object
        this.object = new THREE.Group();

        // Initialize the OBJLoader for loading the 3D model
        this.objLoader = new OBJLoader();

        // Initialize the TextureLoader for loading textures
        this.textureLoader = new THREE.TextureLoader();

        // Load the 3D model using OBJLoader
        this.objLoader.load(
            this.textureUrl, // URL of the OBJ file to load
            (object) => {
                // Set the scale of the loaded object
                object.scale.set(1, 1, 1);

                // Apply a texture or color to each material of the model
                object.traverse((child) => {
                    if (child.isMesh) {
                        // Set a color for the material
                        child.material.color = new THREE.Color(0x616569);
                        child.material.needsUpdate = true; // Ensure the material updates
                    }
                });
                
                // Add the loaded object to the group
                this.object.add(object);
            }
        );
    }
}
