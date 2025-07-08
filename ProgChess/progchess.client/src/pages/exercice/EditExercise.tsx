import { z } from "zod";
import { useEffect, useState, useTransition } from "react";
import { useNavigate, useParams } from "react-router";
import { toast } from "sonner";
import api from "../../utils/api";
import type { Exercise, StudentExercice, TestResult } from "../../utils/type";
import { useFieldArray, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import AdminLayout from "../../components/layout/AdminLayout";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
} from "../../components/ui/form";
import MarkdownComponent from "../../components/form/input/MarkdownComponent";
import { Button } from "../../components/ui/button";
import CodeEditor from "../../components/form/input/CodeEditor";
import { Checkbox } from "../../components/ui/checkbox";
import axios from "axios";
import ExecutionSheet from "../../components/sheet/ExecutionSheet";
import { handleApiError } from "../../utils/apiErrorHandler";
import FileUploader from "../../components/form/input/FileUploader";

const validationSchema = z.object({
  situation: z.string().min(1, {
    message: "Une mise en situation est requise",
  }),
  baseCode: z.string().min(1, {
    message: "Veuillez mettre du code de base",
  }),
  unitTest: z
    .array(
      z.object({
        isActive: z.boolean().optional(),
        code: z.string().min(1, {
          message: "Aucun test n'a été créé",
        }),
      })
    )
    .length(2, { message: "Il peut n'y avoir que 2 type de test" })
    .nonempty({ message: "Au moins un test est requis" }),
  studentCodes: z.string().min(1, {
    message: "Il doit y avoir au moins un étudiant.",
  }),
});

type formSchema = z.infer<typeof validationSchema>;

export default function EditExercise() {
  const [exercise, setExercise] = useState<Exercise>();
  const [situation, setSituation] = useState<string>("");
  const navigate = useNavigate();
  const [isPending, startTransition] = useTransition();
  const { id } = useParams();
  const [openSheet, setOpenSheet] = useState(false);
  const [error, setError] = useState<string>("");
  const [testResult, setTestResult] = useState<TestResult[]>();

  const getExercise = async () => {
    try {
      const response = await api.get(`/api/exercise/${id}`);
      setExercise(response.data);
      setSituation(response.data.situation!);
    } catch (error) {
      handleApiError(error);
    }
  };

  const form = useForm<formSchema>({
    resolver: zodResolver(validationSchema),
    defaultValues: {
      situation: "",
      baseCode: "",
      unitTest: [
        { code: "", isActive: true },
        { code: "", isActive: false },
      ],
      studentCodes: "",
    },
  });

  useEffect(() => {
    getExercise();
  }, []);

  useEffect(() => {
    if (exercise) {
      form.reset({
        situation: exercise.situation,
        baseCode: exercise.baseCode,
        unitTest: exercise.unitTests,
        studentCodes: buildStudentCodeString(exercise.studentExercises),
      });
    }
  }, [exercise]);

  const { fields } = useFieldArray({
    control: form.control,
    name: "unitTest",
    rules: { maxLength: 2 },
  });

  const onSubmit = (values: formSchema) => {
    startTransition(async () => {
      try {
        await api.put(`/api/exercise/edit/${exercise?.id}`, values);
        toast.success("Exercice modifié avec succès !");
        navigate("/admin/exercise");
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const updateSituation = (e: any) => {
    setSituation(e.target.value);
  };

  const buildStudentCodeString = (studentExercises: StudentExercice[]) => {
    return studentExercises
      .map((studentExercise) => studentExercise.student.permanentCode)
      .join("\n");
  };

  const executeCode = () => {
    startTransition(async () => {
      try {
        const code = form.watch("baseCode");
        const unitTest = form
          .watch("unitTest")
          .map((test) => test.code)
          .join("\n");
        const response = await axios.post(
          "http://localhost:5290/api/execute",
          {
            code,
            unitTest,
          },
          { withCredentials: true }
        );
        setOpenSheet(true);
        setTestResult(response.data.value);
        setError("");
      } catch (error) {
        setOpenSheet(true);
        handleApiError(error, setError);
      }
    });
  };
  const handleChildUpload = (newValue: string) => {
    form.setValue(
      "situation",
      form.getValues("situation") +
        `![My Uploaded Image](http://localhost:5290/Image/${newValue})`
    );
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex flex-col w-full">
          <h2 className="text-3xl font-bold text-white">
            Modifier un exercice
          </h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
            <div>
              <h3 className="text-xl font-semibold mb-2">Mise en situation</h3>
              <FileUploader onUpload={handleChildUpload} />
              {form.formState.errors.situation && (
                <span className="text-red-500">
                  {form.formState.errors.situation.message}
                </span>
              )}
              <div className="grid mb-4 grid-cols-2 h-96 border border-zinc-700 rounded-lg overflow-hidden">
                <div className="border-r border-gray-700">
                  <FormField
                    name="situation"
                    control={form.control}
                    render={({ field }) => (
                      <FormItem className="h-full">
                        <FormControl>
                          <textarea
                            className={`w-full h-full p-3 bg-zinc-800 text-white rounded-none focus:outline-none resize-none ${
                              form.formState.errors.situation
                                ? "border border-red-500"
                                : ""
                            }`}
                            value={field.value}
                            onChange={(e) => {
                              field.onChange(e);
                              updateSituation(e);
                            }}
                            placeholder="Écrire en markdown..."
                          ></textarea>
                        </FormControl>
                      </FormItem>
                    )}
                  />
                </div>
                <div className="p-4 overflow-auto bg-zinc-900">
                  <MarkdownComponent markdown={situation} />
                </div>
              </div>{" "}
              {form.formState.errors.situation && (
                <span className="text-red-500">
                  {form.formState.errors.situation.message}
                </span>
              )}
            </div>
            <div className="border-b border-gray-700" />

            <div className="flex items-center justify-between">
              <h3 className="text-xl font-semibold mb-2">Code de base</h3>
              <Button
                type="button"
                className=" bg-green-500 text-white cursor-pointer hover:bg-green-600"
                onClick={() => executeCode()}
              >
                Exécuter
              </Button>
            </div>
            <div className="h-full">
              {form.formState.errors.situation && (
                <span className="text-red-500">
                  {form.formState.errors.situation.message}
                </span>
              )}
              <FormField
                name="baseCode"
                control={form.control}
                render={({ field }) => (
                  <FormItem className="h-full">
                    <FormControl>
                      <CodeEditor
                        value={field.value ?? ""}
                        onChange={field.onChange}
                        placeholder="Code de base pour la situation..."
                        height={window.innerHeight / 2}
                        error={form.formState.errors.baseCode}
                      />
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>

            <div className="border-b border-gray-700" />

            <div className="flex items-center">
              <h3 className="text-xl font-semibold">Tests unitaires</h3>
            </div>
            {fields.length > 0 && (
              <div className="space-y-6">
                {fields.map((field, index) => (
                  <div
                    key={field.id}
                    className="bg-zinc-800 p-4 rounded-lg shadow-lg text-white"
                  >
                    <h3 className="text-xl font-bold">
                      Test {index === 0 ? "visible" : "caché"}
                    </h3>
                    <div className="items-top flex space-x-2 mb-4">
                      <FormField
                        control={form.control}
                        name={`unitTest.${index}.isActive`}
                        render={({ field }) => (
                          <FormItem>
                            <FormControl>
                              <Checkbox
                                hidden
                                checked={field.value}
                                onCheckedChange={field.onChange}
                              />
                            </FormControl>
                          </FormItem>
                        )}
                      />
                    </div>
                    <div className="h-full">
                      {form.formState.errors.situation && (
                        <span className="text-red-500">
                          {
                            form.formState.errors.unitTest?.[index]?.code
                              ?.message
                          }
                        </span>
                      )}
                      <FormField
                        name={`unitTest.${index}.code`}
                        control={form.control}
                        render={({ field }) => (
                          <FormItem className="h-full">
                            <FormControl>
                              <CodeEditor
                                value={field.value ?? ""}
                                onChange={field.onChange}
                                placeholder={`Code de base pour le test ${
                                  index + 1
                                } ...`}
                                height={window.innerHeight / 2}
                                error={
                                  form.formState.errors.unitTest?.[index]?.code
                                }
                              />
                            </FormControl>
                          </FormItem>
                        )}
                      />
                    </div>
                  </div>
                ))}
              </div>
            )}
            <div className="border-b border-gray-700" />

            <div className="space-y-4">
              <div>
                <h3 className="text-xl font-semibold">Liste des étudiants</h3>
                <p className="text-gray-400">
                  <strong>*</strong> Veuillez mettre un code par ligne{" "}
                  <strong>*</strong>
                </p>
                {form.formState.errors.studentCodes && (
                  <span className="text-red-500">
                    {form.formState.errors.studentCodes.message}
                  </span>
                )}
              </div>
              <div className="h-64">
                <FormField
                  name="studentCodes"
                  control={form.control}
                  render={({ field }) => (
                    <FormItem className="h-full">
                      <FormControl>
                        <textarea
                          className={`w-full h-full p-3 border  text-white rounded focus:outline-none resize-none ${
                            form.formState.errors.studentCodes
                              ? "border border-red-500"
                              : "border-zinc-700 bg-zinc-800"
                          }`}
                          value={field.value}
                          onChange={field.onChange}
                        ></textarea>
                      </FormControl>
                    </FormItem>
                  )}
                />
              </div>
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
      <ExecutionSheet
        open={openSheet}
        onOpenChange={setOpenSheet}
        error={error}
        testResult={testResult}
      />
    </AdminLayout>
  );
}
