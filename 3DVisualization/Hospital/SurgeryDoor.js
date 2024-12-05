// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a wall with a door frame, including 
//              texture application and geometry creation for the wall and door frame faces.
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  textureUrl: String, // URL of the texture for the wall
 *  width: Number, // Width of the wall with door frame
 *  height: Number, // Height of the wall with door frame
 * }
 */

export default class WallWithDoorFrame {
    /**
     * Constructor for the WallWithDoorFrame class.
     * 
     * @param {Object} parameters - Configuration object for the wall with door frame.
     */
    constructor(parameters) {
        // Dynamically assign the provided parameters to the instance
        for (const [key, value] of Object.entries(parameters)) {
            this[key] = value;
        }

        // Load the texture for the wall
        const texture = new THREE.TextureLoader().load(this.textureUrl);
        texture.colorSpace = THREE.SRGBColorSpace;
        texture.magFilter = THREE.LinearFilter;
        texture.minFilter = THREE.LinearMipmapLinearFilter;

        // Create a group to hold the wall and its components
        this.object = new THREE.Group();

        // Define the height and width of the wall
        const height = 4;
        const width = 1;
        const halfheight = (height / 2 - 0.5);

        // Create the front face of the wall (a rectangle)
        let geometry = new THREE.PlaneGeometry(width, height);
        let material = new THREE.MeshPhongMaterial({
            color: 0x9cacb2, // Set the color of the wall
            map: texture,   // Apply the texture to the wall
            side: THREE.DoubleSide, // Apply the texture to both sides
            transparent: true,
            side: THREE.FrontSide
        });
        let face = new THREE.Mesh(geometry, material);
        face.position.set(0.0, halfheight, 0.025); // Position the front face
        face.castShadow = true;  // Allow the front face to cast shadows
        face.receiveShadow = true; // Allow the front face to receive shadows
        this.object.add(face); // Add the front face to the group

        // Create the rear face of the wall (a rectangle)
        const rearMaterial = material.clone();  // Clone the material to modify the texture separately
        rearMaterial.map = texture.clone(); // Clone the texture for the back face
        rearMaterial.map.wrapS = THREE.RepeatWrapping; // Set the texture wrapping mode
        rearMaterial.map.repeat.x = -1;  // Flip the texture horizontally for the back face
        face = new THREE.Mesh(geometry, rearMaterial);
        face.rotateY(Math.PI); // Rotate the rear face 180 degrees
        face.position.set(0.0, halfheight, -0.025); // Position the rear face
        this.object.add(face); // Add the rear face to the group

        // Create the left face of the wall (a rectangle)
        geometry = new THREE.PlaneGeometry(0.05, height); // Create a thin rectangle for the left face
        material = new THREE.MeshPhongMaterial({ color: 0x9cacb2, side: THREE.DoubleSide}); // Use the same material
        face = new THREE.Mesh(geometry, material);
        face.position.set(-width / 2, halfheight, 0); // Position the left face
        face.rotateY(Math.PI / 2); // Rotate the left face by 90 degrees
        face.castShadow = true; // Allow the left face to cast shadows
        face.receiveShadow = true; // Allow the left face to receive shadows
        this.object.add(face); // Add the left face to the group

        // Create the right face of the wall (a rectangle)
        face = new THREE.Mesh().copy(face, false); // Copy the left face and invert it for the right side
        face.position.set(width / 2, halfheight, 0); // Position the right face
        this.object.add(face); // Add the right face to the group

        // Create the top face of the wall (a rectangle)
        geometry = new THREE.PlaneGeometry(width, 0.05); // Create a thin rectangle for the top face
        face = new THREE.Mesh(geometry, material);
        face.position.set(0, height - 0.5, 0); // Position the top face
        face.rotateX(-Math.PI / 2); // Rotate the top face by 90 degrees to lie horizontally
        face.castShadow = true; // Allow the top face to cast shadows
        face.receiveShadow = true; // Allow the top face to receive shadows
        this.object.add(face); // Add the top face to the group
    }
}
