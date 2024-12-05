import React, { useState } from 'react';
import './OperationTypeList.css';

const OperationTypesList = ({ operationTypes, onSelect }) => {
    const [selectedId, setSelectedId] = useState(null);

    const handleSelect = (type) => {
        setSelectedId(type.id);
        if (onSelect) {
            onSelect(type);
        }
    };

    return (
        <div className="operation-types-list-container">
            <ul className="operation-types-list">
                {operationTypes.map((type) => (
                    <li
                        key={type.id}
                        className={`operation-type-item ${selectedId === type.id ? 'selected' : ''}`}
                        onClick={() => handleSelect(type)}
                    >
                        {type.name}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OperationTypesList;