// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a 3D model of a surgeon, including texture 
//              and OBJ model loading, applied to a Three.js scene.
// ============================================================================

import * as THREE from "three";
import { OBJLoader } from 'three/addons/loaders/OBJLoader.js';

/*
 * parameters = {
 *  surgeonTextureUrl: String, // URL of the surgeon's texture
 *  surgeonModelOBJUrl: String, // URL of the surgeon's OBJ model file
 *  scale: Number, // Optional scale value
 * }
 */

export default class Surgeon {
    /**
     * Constructor for the Surgeon class.
     * 
     * @param {Object} parameters - Configuration object for the Surgeon.
     */
    constructor(parameters) {
        // Dynamically assign the provided parameters to the instance
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }
        
        // Create a group to hold the surgeon model and its components
        this.object = new THREE.Group();

        // Instantiate the OBJLoader to load the surgeon model
        this.objLoader = new OBJLoader();

        // Instantiate the TextureLoader to load the texture
        this.textureLoader = new THREE.TextureLoader();

        // Load the texture for the surgeon
        this.texture = this.textureLoader.load(this.surgeonTextureUrl);

        // Load the surgeon OBJ model
        this.objLoader.load(
            this.surgeonModelOBJUrl, // URL of the OBJ file
            (object) => {
                // Scale the surgeon model to the desired size
                object.scale.set(0.01, 0.02, 0.01);

                // Apply the loaded texture to each material of the model
                object.traverse((child) => {
                    if (child.isMesh) {
                        child.material.map = this.texture; // Assign the texture to the material
                        child.material.needsUpdate = true; // Update the material with the new texture
                    }
                });

                // Add the loaded surgeon object to the group
                this.object.add(object);
            },
            undefined, // onProgress function (not used in this case)
            (error) => {
                // Handle errors during the model loading process
                console.error('An error occurred while loading the surgeon model:', error);
            }
        );
    }
}
