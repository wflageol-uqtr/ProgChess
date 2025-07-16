import type { SetStateAction } from "react";
import { toast } from "sonner";

export const handleApiError = (error: any, setState?: SetStateAction<any>) => {
    if (error) {        
        const statusCode = error?.status || error?.error?.status || 500;
        const errorMessage = error.response?.data?.detail || error.message || "Erreur serveur est survenue";        
        
        if ((statusCode === 400 || statusCode === 600) && setState) {
            setState(errorMessage);
            return;
        }
        if (statusCode == 429 && setState){
            setState("Trop de tentatives de connexion. Veuillez réessayer dans 1 minute.");
            return;
        }
        toast.error(errorMessage);
    }
}