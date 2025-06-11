import type { SetStateAction } from "react";
import { toast } from "sonner";

export const handleApiError = (error: any, setState?: SetStateAction<any>) => {
    if (error) {    
        const statusCode = error.status || error.status || 500;
        const errorMessage = error.response.data.detail || error.message || "Erreur serveur est survenue";        

        if (statusCode === 400 && setState) {
            setState(errorMessage)
        } else {
            toast.error(errorMessage);
       }
    }
}