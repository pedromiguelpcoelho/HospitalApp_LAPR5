// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a bed in a 3D hospital scene, including 
//              its texture, scale, and rotation, using the three.js library.
// ============================================================================

import * as THREE from "three";
import { OBJLoader } from 'three/addons/loaders/OBJLoader.js';

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture to be applied to the object
 *  scale: Number, // Scale factor for the object
 *  rotation: { x: Number, y: Number, z: Number } // Rotation angles in radians for each axis
 * }
 */

export default class Bed {
    /**
     * Constructor for the Bed class.
     * 
     * @param {Object} parameters - Configuration object for the Bed.
     */
    constructor(parameters) {
        // Assign the provided parameters to the instance dynamically
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        
        // Create a Three.js group to hold the bed object
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
                object.scale.set(0.007, 0.014, 0.007);

                // Apply a texture or color to each material of the model
                object.traverse((child) => {
                    if (child.isMesh) {
                        // Set a color for the material (example: light brown)
                        child.material.color = new THREE.Color(0x8E9DAB);
                        child.material.needsUpdate = true; // Ensure the material updates
                    }
                });
                
                // Add the loaded object to the group
                this.object.add(object);
            }
        );
    }
}
