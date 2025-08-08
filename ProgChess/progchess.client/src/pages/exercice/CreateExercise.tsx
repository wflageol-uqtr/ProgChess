import { z } from "zod";
import { useState, useTransition } from "react";
import MarkdownComponent from "../../components/form/input/MarkdownComponent";
import AdminLayout from "../../components/layout/AdminLayout";
import { Button } from "../../components/ui/button";
import { Checkbox } from "../../components/ui/checkbox";
import CodeEditor from "../../components/form/input/CodeEditor";
import { useFieldArray, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
} from "../../components/ui/form";
import api, { apiUrl } from "../../utils/api";
import { toast } from "sonner";
import { useNavigate } from "react-router";
import axios from "axios";
import ExecutionSheet from "../../components/sheet/ExecutionSheet";
import type {
  TestResult,
  Image,
  EditorInfo,
  EditorError,
} from "../../utils/type";
import {
  handleApiError,
  handleExecutionError,
} from "../../utils/apiErrorHandler";
import ImageSelector from "../../components/form/select/ImageSelector";
import ProgChessLoader from "../../components/loader/ProgChessLoader";

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

export default function CreateExercise() {
  const navigate = useNavigate();
  const [isPending, startTransition] = useTransition();
  const [isExecuting, startExecution] = useTransition();
  const [situation, setSituation] = useState<string>("");
  const [openSheet, setOpenSheet] = useState(false);
  const [testResult, setTestResult] = useState<TestResult[]>();
  const [errorEditor, setErrorEditor] = useState<EditorError>();
  const editorInfo: EditorInfo[] = [];

  const updateSituation = (e: any) => {
    setSituation(e.target.value);
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

  const { fields } = useFieldArray({
    control: form.control,
    name: "unitTest",
    rules: { maxLength: 2 },
  });

  const onSubmit = (values: formSchema) => {
    startTransition(async () => {
      try {
        await api.post("/api/exercise/create", values);
        toast.success("Exercice créé avec succès !");
        navigate("/admin/exercise");
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  const executeCode = () => {
    startExecution(async () => {
      setErrorEditor(undefined);
      try {
        const code = form.watch("baseCode");
        editorInfo.push({
          name: "code",
          size: countLines(code),
        });

        const unitTest = form
          .watch("unitTest")
          .map((test, index) => {
            editorInfo.push({
              name: `test${index}`,
              size: countLines(test.code),
            });
            return test.code;
          })
          .join("\n");
        const response = await axios.post(
          `${apiUrl}/api/execute`,
          {
            code,
            unitTest,
          },
          { withCredentials: true }
        );
        setOpenSheet(true);
        setTestResult(response.data.value);
      } catch (error: any) {
        if (error?.status === 600) {
          const errorObj = handleExecutionError(
            editorInfo,
            error.response?.data?.detail
          );
          setErrorEditor(errorObj);
          toast.error("Une erreur d'exécution est survenue");
          return;
        }
        handleApiError(error);
      }
    });
  };

  const handleSelectedImage = (image?: Image) => {
    if (image) {
      form.setValue(
        "situation",
        form.getValues("situation") +
          `![${image.name}](${apiUrl}/${image.path})`
      );
    }
  };

  const countLines = (str: string) => {
    if (str.trim() === "") return 0;
    return str.split("\n").length;
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex flex-col w-full">
          <h2 className="text-2xl font-bold text-white">Ajouter un exercice</h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
            <div>
              <h3 className="text-lg md:text-xl font-semibold mb-2">
                Mise en situation
              </h3>
              <div className="flex items-center justify-end gap-4 my-1">
                <ImageSelector onSelectedImage={handleSelectedImage} />
              </div>
              {form.formState.errors.situation && (
                <span className="text-red-500">
                  {form.formState.errors.situation.message}
                </span>
              )}
              <div className="grid mb-4 grid-cols-1 md:grid-cols-2 border border-zinc-700 rounded-lg overflow-hidden">
                <div className="flex flex-col border-b md:border-b-0 md:border-r border-gray-700">
                  <FormField
                    name="situation"
                    control={form.control}
                    render={({ field }) => (
                      <FormItem className="flex-1">
                        <FormControl>
                          <textarea
                            className={`w-full p-3 bg-zinc-800 h-48 md:h-96 text-white focus:outline-none resize-none over md:overflow-auto ${
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
                          />
                        </FormControl>
                      </FormItem>
                    )}
                  />
                </div>
                <div className="p-4 h-48 md:h-96 overflow-auto bg-zinc-900">
                  <MarkdownComponent markdown={situation} />
                </div>
              </div>
            </div>
            <div className="border-b border-gray-700" />

            <div className="flex items-center justify-between">
              <h3 className="text-lg md:text-xl font-semibold mb-2">
                Code de base
              </h3>
              <Button
                type="button"
                className=" bg-green-500 text-white cursor-pointer hover:bg-green-600"
                onClick={() => executeCode()}
                disabled={isExecuting}
              >
                Exécuter
              </Button>
            </div>
            <div className="h-full">
              {form.formState.errors.baseCode && (
                <span className="text-red-500">
                  {form.formState.errors.baseCode.message}
                </span>
              )}
              <FormField
                name="baseCode"
                control={form.control}
                render={({ field }) => (
                  <FormItem className="h-full">
                    <FormControl>
                      <div className="relative">
                        <CodeEditor
                          value={field.value ?? ""}
                          onChange={field.onChange}
                          placeholder="Code de base pour la situation..."
                          height={window.innerHeight / 2}
                          error={form.formState.errors.baseCode}
                          executionError={
                            errorEditor?.id === "code" ? errorEditor : undefined
                          }
                        />
                        {isExecuting && <ProgChessLoader />}
                      </div>
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>

            <div className="border-b border-gray-700" />

            <div className="flex items-center">
              <h3 className="text-lg md:text-xl font-semibold">
                Tests unitaires
              </h3>
            </div>
            {fields.length > 0 && (
              <div className="space-y-6">
                {fields.map((field, index) => (
                  <div
                    key={field.id}
                    className="bg-zinc-800 p-4 rounded-lg shadow-lg text-white"
                  >
                    <h3 className="text-lg md:text-xl font-bold">
                      Test {index === 0 ? "visible" : "caché"}
                    </h3>
                    <div className="flex">
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
                                placeholder={`Code de base pour les tests ${
                                  index === 0 ? "visibles" : "cachés"
                                } ...`}
                                height={window.innerHeight / 2}
                                error={
                                  form.formState.errors.unitTest?.[index]?.code
                                }
                                executionError={
                                  errorEditor?.id === `test${index}`
                                    ? errorEditor
                                    : undefined
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
                <h3 className="text-lg md:text-xl font-semibold">
                  Liste des étudiants
                </h3>
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
        testResult={testResult}
      />
    </AdminLayout>
  );
}
