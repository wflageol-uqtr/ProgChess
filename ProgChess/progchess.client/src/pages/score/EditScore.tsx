import { useState, useTransition } from "react";
import { useNavigate, useParams } from "react-router";
import { z } from "zod";
import type { Exercise } from "../../utils/type";
import api from "../../utils/api";
import { handleApiError } from "../../utils/apiErrorHandler";

const validationSchema = z.object({
  permanentCode: z.string().min(12, "Code de l'étudiant requis"),
  exerciseId: z.number().min(1, { message: "Numéro d'exercice invalide" }),
  scoreValue: z.number({ message: "Un chiffre est requis" }),
  answer: z.string(),
});

type formSchema = z.infer<typeof validationSchema>;

export default function EditScore() {
  const navigate = useNavigate();
  const [isPending, startTransition] = useTransition();
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [currentExercise, setCurrentExercise] = useState<Exercise>();
  const { id } = useParams();

  const getScore = async () => {
    try {
      const response = await api.get(`/api/score/${id}`);
      //   setExercise(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  return <div>modifier {id}</div>;
}
