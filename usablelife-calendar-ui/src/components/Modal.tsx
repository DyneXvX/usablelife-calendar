import type { ReactNode } from 'react';

interface ModalProps {
    onClose: () => void;
    children: ReactNode;
}

/**
 * A popup box that floats over the page, centered, with a dimmed backdrop.
 * Clicking the backdrop closes it.
 */
function Modal({ onClose, children }: ModalProps) {
    return (
        <div className="modal-backdrop" onClick={onClose}>
            <div className="modal" onClick={(e) => e.stopPropagation()}>
                {children}
            </div>
        </div>
    );
}

export default Modal;
