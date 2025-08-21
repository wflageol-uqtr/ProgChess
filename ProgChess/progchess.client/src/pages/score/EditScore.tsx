import { useEffect, useState, useTransition } from "react";
import { useNavigate, useParams } from "react-router";
import { z } from "zod";
import type { Exercise, Score, StudentExercice } from "../../utils/type";
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../components/ui/select";
import { useFieldArray, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Checkbox } from "../../components/ui/checkbox";
import { Label } from "../../components/ui/label";
import { Input } from "../../components/ui/input";
import { toast } from "sonner";

const validationSchema = z.object({
  studentExerciseId: z
    .number()
    .min(1, { message: "Numéro de l'étudiant invalide" }),
  exerciseId: z.number().min(1, { message: "Numéro d'exercice invalide" }),
  answer: z.string(),
  isComplete: z.boolean().optional(),
  scoreTests: z.array(
    z.object({
      name: z.string().min(1, { message: "Nom obligatoire" }),
      isSuccess: z.boolean().optional(),
      actual: z.string().optional().nullable(),
      expected: z.string().optional().nullable(),
    })
  ),
});

type formSchema = z.infer<typeof validationSchema>;

export default function EditScore() {
  const navigate = useNavigate();
  const [isPending, startTransition] = useTransition();
  const [exercises, setExercises] = useState<Exercise[]>([]);
  const [score, setScore] = useState<Score>();
  const [studentExercises, setStudentExercises] = useState<StudentExercice[]>(
    []
  );
  const [currentExercise, setCurrentExercise] = useState<Exercise>();
  const { id } = useParams();

  const form = useForm<formSchema>({
    resolver: zodResolver(validationSchema),
    defaultValues: {
      studentExerciseId: 0,
      exerciseId: 0,
      answer: "",
      isComplete: true,
      scoreTests: [{ name: "", isSuccess: false, expected: "", actual: "" }],
    },
  });

  const [exerciseId, studentId] = form.watch([
    "exerciseId",
    "studentExerciseId",
  ]);

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: "scoreTests",
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

  const getAllStudentExercise = async () => {
    try {
      const response = await api.get("/api/studentexercise");
      setStudentExercises(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getAllExercise();
    getScore();
    getAllStudentExercise();
  }, []);

  useEffect(() => {
    if (score) {
      form.reset({
        studentExerciseId: score?.studentExerciseId,
        exerciseId: score?.exercise.id,
        answer: score?.answer,
        scoreTests: score?.scoreTests,
      });
      var currentExercise = exercises.find(
        (exercise) => exercise.id == score.exercise.id
      );
      setCurrentExercise(currentExercise);
    }
  }, [score, exercises]);

  useEffect(() => {
    if (score) {
      var result = studentExercises.find(
        (item) =>
          item.exerciseId === Number(exerciseId) &&
          item.id === Number(studentId)
      );
      form.setValue("isComplete", result?.isComplete);
    }
  }, [studentId, exerciseId]);

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
          <h2 className="text-2xl font-bold text-white">
            Modifier un résultat
          </h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-8" onSubmit={form.handleSubmit(onSubmit)}>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <h3 className="text-lg md:text-xl font-semibold mb-2">
                  Exercice
                </h3>
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
                <h3 className="text-lg md:text-xl font-semibold mb-2">
                  Code permanent
                </h3>
                <FormField
                  control={form.control}
                  name="studentExerciseId"
                  render={({ field }) => (
                    <FormItem>
                      <Select
                        onValueChange={(id) => {
                          const parsed = parseInt(id);
                          if (!isNaN(parsed)) {
                            field.onChange(parsed);
                          }
                        }}
                        value={field.value?.toString()}
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
                                  value={item.id.toString()}
                                >
                                  {item.studentPermanentCode}
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
              <h3 className="text-lg md:text-xl font-semibold mb-2">Score</h3>
              <FormField
                control={form.control}
                name="isComplete"
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <div className="flex items-center gap-2">
                        <Checkbox
                          id="isComplete"
                          checked={field.value}
                          onCheckedChange={field.onChange}
                        />
                        <Label htmlFor="isComplete">
                          L'étudiant a complété l'exercice
                        </Label>
                      </div>
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>
            <div className="border-b border-gray-700" />
            <div className="flex justify-between items-center">
              <h3 className="text-lg md:text-xl font-semibold mb-2">
                Test du score
              </h3>
              <Button
                type="button"
                className="bg-green-500 hover:bg-green-600 text-xl cursor-pointer"
                onClick={() =>
                  append({
                    name: "",
                    isSuccess: false,
                    expected: "",
                    actual: "",
                  })
                }
              >
                +
              </Button>
            </div>
            <div className="space-y-4">
              {fields.map((field, index) => (
                <div
                  key={field.id}
                  className={`relative p-5 rounded-2xl shadow-md space-y-6 border border-zinc-700`}
                >
                  <div
                    className="absolute size-6 -top-2 -right-1 text-gray-500 border border-gray-500 hover:text-red-600 hover:border-red-600 transition-colors duration-200 rounded-full text-center cursor-pointer"
                    onClick={() => remove(index)}
                  >
                    X
                  </div>
                  <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                    <div className="flex-1">
                      <label className="text-sm font-medium text-zinc-300 mb-1 block">
                        Nom du test
                      </label>
                      <FormField
                        control={form.control}
                        name={`scoreTests.${index}.name`}
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input
                                placeholder="Nom du test"
                                type="text"
                                className="w-full"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage className="text-red-600" />
                          </FormItem>
                        )}
                      />
                    </div>

                    <FormField
                      control={form.control}
                      name={`scoreTests.${index}.isSuccess`}
                      render={({ field }) => (
                        <FormItem>
                          <FormControl>
                            <div className="flex items-center space-x-2 mt-1 sm:mt-6">
                              <Checkbox
                                id={`isSuccess-${index}`}
                                checked={field.value}
                                onCheckedChange={field.onChange}
                              />
                              <Label
                                htmlFor={`isSuccess-${index}`}
                                className="text-sm font-medium text-zinc-300"
                              >
                                Réussie
                              </Label>
                            </div>
                          </FormControl>
                        </FormItem>
                      )}
                    />
                  </div>

                  <hr className="border-zinc-700" />

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                      <label className="text-sm font-medium text-zinc-300 mb-1 block">
                        Actuel (optionnel)
                      </label>
                      <FormField
                        control={form.control}
                        name={`scoreTests.${index}.actual`}
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input
                                placeholder="Valeur actuelle"
                                type="text"
                                className="w-full"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage className="text-red-600" />
                          </FormItem>
                        )}
                      />
                    </div>

                    <div>
                      <label className="text-sm font-medium text-zinc-300 mb-1 block">
                        Attendue (optionnel)
                      </label>
                      <FormField
                        control={form.control}
                        name={`scoreTests.${index}.expected`}
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Input
                                placeholder="Valeur attendue"
                                type="text"
                                className="w-full"
                                {...field}
                              />
                            </FormControl>
                            <FormMessage className="text-red-600" />
                          </FormItem>
                        )}
                      />
                    </div>
                  </div>
                </div>
              ))}
            </div>

            <div className="border-b border-gray-700" />

            <div>
              <h3 className="text-lg md:text-xl font-semibold mb-2">
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
