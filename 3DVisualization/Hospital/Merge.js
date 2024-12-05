// ============================================================================
// Author: Team 44
// Date: 2024-11-15
// Description: This function merges multiple objects into a single object, 
//              with special handling for arrays and read-only properties.
// Adapted from: Basic_Thumb_Raiser (SGRAI) 
// ============================================================================

/**
 * Merges multiple objects into the first one, with special handling for arrays and read-only properties.
 * 
 * @param {Object} object - The target object to merge into.
 * @param {...Object} sources - The source objects to merge from.
 * @returns {Object} The merged object.
 */
export function merge(object, ...sources) {
    return _.mergeWith(object, ...sources, (objValue, srcValue, key, object, source) => {
        // If the value in the object is an array, concatenate the arrays
        if (_.isArray(objValue)) {
            return objValue.concat(srcValue); // Concatenate arrays
        } else {
            // If the property is read-only (e.g., in Three.js like position or scale), handle it
            const descriptor = Object.getOwnPropertyDescriptor(object, key);
            
            if (descriptor !== undefined && !descriptor.writable) {
                // Temporarily set the property to writable to allow modification
                descriptor.value = srcValue;
                descriptor.writable = true; // Set property as writable
                Object.defineProperty(object, key, descriptor); // Define property as writable
                
                // Set the property back to read-only after modifying it
                descriptor.writable = false; // Make it read-only again
                Object.defineProperty(object, key, descriptor); // Reapply the read-only descriptor
            }
        }
    });
}
