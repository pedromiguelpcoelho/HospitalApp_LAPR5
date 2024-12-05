// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This class represents a surgery room door with two faces (front and back), 
//              using a texture applied to both sides, and positioning adjustments 
//              to avoid z-fighting issues.
// ============================================================================

import * as THREE from "three";

/*
 * parameters = {
 *  textureUrl: string, // URL of the texture to apply to the door
 *  width: number, // Width of the door
 *  height: number, // Height of the door
 * }
 */

export default class SurgeryRoomDoor {
    /**
     * Constructor for the SurgeryRoomDoor class.
     * 
     * @param {Object} parameters - Configuration object for the door.
     */
    constructor(parameters) {
        // Destructure the parameters object to assign individual values
        this.textureUrl = parameters.textureUrl;
        this.width = parameters.width;
        this.height = parameters.height;

        // Load the texture for the door
        const texture = new THREE.TextureLoader().load(this.textureUrl);
        texture.colorSpace = THREE.SRGBColorSpace; // Ensure the texture uses the correct color space
        texture.magFilter = THREE.LinearFilter; // Set the texture filtering mode
        texture.minFilter = THREE.LinearMipmapLinearFilter; // Set the mipmap filter mode

        // Create a group to hold the door and its components
        this.object = new THREE.Group();

        // Define the height of the wall and the door height
        const wallHeight = 5; // Height of the wall
        const doorHeight = 4;  // Height of the door itself

        // Create geometry for the door's face (a rectangle)
        const geometry = new THREE.PlaneGeometry(this.width, doorHeight);

        // Create the material for the front face of the door
        const frontMaterial = new THREE.MeshPhongMaterial({
            color: 0xffffff,  // Use white as the base color
            map: texture,     // Apply the loaded texture
            transparent: true, // Allow transparency (useful for textures with alpha)
            side: THREE.FrontSide // Only display the front side of this face
        });

        // Create the material for the back face of the door
        const backMaterial = new THREE.MeshPhongMaterial({
            color: 0xffffff,  // Use white as the base color
            map: texture,     // Apply the same texture as the front face
            transparent: true, // Allow transparency
            side: THREE.BackSide // Only display the back side of this face
        });

        // Create the front face of the door and position it slightly forward
        const frontFace = new THREE.Mesh(geometry, frontMaterial);
        frontFace.position.set(0, 1.5, 0.01); // Slight offset on the z-axis to avoid z-fighting
        frontFace.castShadow = true; // Allow the front face to cast shadows
        frontFace.receiveShadow = true; // Allow the front face to receive shadows

        // Create the back face of the door and position it slightly backward
        const backFace = new THREE.Mesh(geometry, backMaterial);
        backFace.position.set(0, 1.5, -0.01); // Slight offset on the z-axis to avoid z-fighting
        backFace.castShadow = true; // Allow the back face to cast shadows
        backFace.receiveShadow = true; // Allow the back face to receive shadows

        // Rotate both the front and back faces by 180 degrees on the y-axis
        frontFace.rotation.y = Math.PI; // Rotate the front face by 180 degrees
        backFace.rotation.y = Math.PI; // Ensure the back face matches the front face's rotation

        // Add both faces (front and back) to the door group
        this.object.add(frontFace);
        this.object.add(backFace);
    }
}
