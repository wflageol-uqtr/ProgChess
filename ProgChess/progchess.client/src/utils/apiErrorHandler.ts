import type { SetStateAction } from "react";
import { toast } from "sonner";
import type { EditorError, EditorInfo } from "./type";

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

export const handleExecutionError = (editorInfo: EditorInfo[], detail: string): EditorError => {
    const match = detail?.match(/Error at line:(\d+)/);
    const elementToRemove = match?.[0] ?? "";
    const lineNumber = match ? parseInt(match[1], 10) : 1;
    let currentLine = 1;    
    
    for (const info of editorInfo) {
        const nextStart = currentLine + info.size;

        if (lineNumber >= currentLine && lineNumber < nextStart) {
            return {
                id: info.name,
                line: lineNumber - currentLine + 1,
                error: detail.replace(elementToRemove, "")
            };
        }

        currentLine = nextStart;
    }
    return { id: "code", line: 1, error: detail.replace(elementToRemove, "") };
}