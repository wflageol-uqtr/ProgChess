import { useEffect, useState, useTransition } from "react";
import { useNavigate, useParams } from "react-router";
import { z } from "zod";
import type { Exercise, Score } from "../../utils/type";
import api from "../../utils/api";
import { handleApiError } from "../../utils/apiErrorHandler";
import AdminLayout from "../../components/layout/AdminLayout";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "../../components/ui/form";
import { Button } from "../../components/ui/button";
import CodeEditor from "../../components/form/input/CodeEditor";
import { Input } from "../../components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../components/ui/select";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "sonner";

const validationSchema = z.object({
  studentId: z.number().min(1, { message: "Numéro de l'étudiant invalide" }),
  exerciseId: z.number().min(1, { message: "Numéro d'exercice invalide" }),
  scoreValue: z.number({ message: "Un chiffre est requis" }),
  answer: z.string(),
});

type formSchema = z.infer<typeof validationSchema>;

export default function EditScore() {
  const navigate = useNavigate();
  const [isPending, startTransition] = useTransition();
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [score, setScore] = useState<Score>();
  const [currentExercise, setCurrentExercise] = useState<Exercise>();
  const { id } = useParams();

  const form = useForm<formSchema>({
    resolver: zodResolver(validationSchema),
    defaultValues: {
      studentId: 0,
      exerciseId: 0,
      scoreValue: 0,
      answer: "",
    },
  });

  const getScore = async () => {
    try {
      const response = await api.get(`/api/score/${id}`);
      setScore(response.data);
    } catch (error: any) {
      handleApiError(error.error);
    }
  };

  const getAllExercise = async () => {
    try {
      const response = await api.get("/api/exercise");
      setExercises(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getAllExercise();
    getScore();
  }, []);

  useEffect(() => {
    if (score) {
      form.reset({
        studentId: score?.student.id,
        exerciseId: score?.exercise.id,
        scoreValue: score?.scoreValue,
        answer: score?.answer,
      });
      var currentExercise = exercises.find(
        (exercise) => exercise.id == score.exercise.id
      );
      setCurrentExercise(currentExercise);
    }
  }, [score, exercises]);

  const onSubmit = (values: formSchema) => {
    startTransition(async () => {
      try {
        await api.put(`/api/score/edit/${id}`, values);
        toast.success("Score modifié avec succès !");
        navigate("/admin/score");
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const formattedSituation = (situation: string) => {
    if (typeof situation === "string") {
      return situation.length > 20
        ? situation.substring(0, 23) + "..."
        : situation;
    } else {
      return String(situation ?? "");
    }
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex">
          <h2 className="text-2xl font-bold text-white">Modifier un score</h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-8" onSubmit={form.handleSubmit(onSubmit)}>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <h3 className="text-xl font-semibold mb-2">Exercice</h3>
                <FormField
                  control={form.control}
                  name="exerciseId"
                  render={({ field }) => (
                    <FormItem>
                      <Select
                        onValueChange={(id) => {
                          field.onChange(parseInt(id));
                          const selected = exercises.find(
                            (exercise) => exercise.id === parseInt(id)
                          );
                          setCurrentExercise(selected);
                        }}
                        value={field.value?.toString()}
                      >
                        <FormControl>
                          <SelectTrigger className="w-full">
                            <SelectValue placeholder="Choisir un exercice..." />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent className="bg-zinc-950 text-white">
                          {exercises.map((exercise) => (
                            <SelectItem
                              key={exercise.id}
                              value={exercise.id.toString()}
                            >
                              {formattedSituation(exercise.situation)}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      <FormMessage className="text-red-600" />
                    </FormItem>
                  )}
                />
              </div>

              <div>
                {/* Explorer ce qui se passe ici  */}
                <h3 className="text-xl font-semibold mb-2">Code permanent</h3>
                <FormField
                  control={form.control}
                  name="studentId"
                  render={({ field }) => (
                    <FormItem>
                      <Select
                        onValueChange={(id) => field.onChange(parseInt(id))}
                        value={field.value.toString()}
                      >
                        <FormControl>
                          <SelectTrigger className="w-full">
                            <SelectValue placeholder="Choisir un code permanent..." />
                          </SelectTrigger>
                        </FormControl>
                        <SelectContent className="bg-zinc-950 text-white">
                          {currentExercise?.studentExercises?.length! > 0 ? (
                            currentExercise?.studentExercises.map(
                              (item, index) => (
                                <SelectItem
                                  key={index}
                                  value={item.studentId.toString()}
                                >
                                  {item.student.permanentCode}
                                </SelectItem>
                              )
                            )
                          ) : (
                            <div className="text-sm p-2 text-white">
                              Pas d'étudiant associé
                            </div>
                          )}
                        </SelectContent>
                      </Select>
                      <FormMessage className="text-red-600" />
                    </FormItem>
                  )}
                />
              </div>
            </div>
            <div className="border-b border-gray-700" />

            <div>
              <h3 className="text-xl font-semibold mb-2">Score</h3>
              <FormField
                control={form.control}
                name="scoreValue"
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <div className="flex items-center gap-2 flex-wrap">
                        <p>L'étudiant a obtenu un score de :</p>
                        <Input
                          placeholder="0"
                          type="text"
                          className="w-16 text-center"
                          value={field.value}
                          onChange={(e) =>
                            field.onChange(
                              e.target.value === ""
                                ? ""
                                : Number(e.target.value)
                            )
                          }
                        />
                        <p>/ X</p>
                      </div>
                    </FormControl>
                    <FormMessage className="text-red-600" />
                  </FormItem>
                )}
              />
            </div>
            <div className="border-b border-gray-700" />

            <div>
              <h3 className="text-xl font-semibold mb-2">
                Réponse de l'étudiant
              </h3>

              <FormField
                name={"answer"}
                control={form.control}
                render={({ field }) => (
                  <FormItem className="h-full">
                    <FormControl>
                      <CodeEditor
                        value={field.value ?? ""}
                        onChange={field.onChange}
                        height={window.innerHeight / 2}
                        error={form.formState.errors.answer}
                      />
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>
            <div className="border-b border-gray-700" />

            <div className="flex justify-end h-12">
              <Button
                type="submit"
                className="bg-green-600 cursor-pointer hover:bg-green-700 text-white font-semibold py-2 px-4 rounded-lg"
              >
                {isPending ? "Soummission..." : "Soummettre"}
              </Button>
            </div>
          </form>
        </Form>
      </div>
    </AdminLayout>
  );
}
