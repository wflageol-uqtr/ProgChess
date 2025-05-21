import { z } from "zod";
import { startTransition, useState } from "react";
import MarkdownComponent from "../../components/form/input/MarkdownComponent";
import AdminLayout from "../../components/layout/AdminLayout";
import { Button } from "../../components/ui/button";
import { Trash } from "lucide-react";
import { Checkbox } from "../../components/ui/checkbox";
import CodeEditor from "../../components/form/input/CodeEditor";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "../../components/ui/form";
import { api } from "../../utils/api";

const validationSchema = z.object({
  situation: z.string().min(1, {
    message: "Une mise en situation est requise !",
  }),
  code: z.string().optional(),
  // tests: z
  //   .array(
  //     z.object({
  //       isHidden: z.boolean().optional(),
  //       code: z.string().optional(),
  //     })
  //   )
  //   .nonempty({ message: "Au moins un test est requis" }),
});

type formSchema = z.infer<typeof validationSchema>;

export default function CreateExercice() {
  const form = useForm<formSchema>({
    resolver: zodResolver(validationSchema),
    defaultValues: {
      situation: "",
      code: "",
      // tests: [],
    },
  });

  const onSubmit = (values: formSchema) => {
    startTransition(async () => {
      try {
        const response = await api.post("/api/exercice/create", values);
        console.log(response);
      } catch (error) {
        console.log(error);
      }
    });
  };

  const [allTests, setAllTests] = useState([{ code: "", isHidden: false }]);
  const [situation, setSituation] = useState<string>("");

  const updateSituation = (e: any) => {
    setSituation(e.target.value);
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex flex-col w-full">
          <h2 className="text-3xl font-bold text-white">Ajouter un exercice</h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
            <div>
              <h3 className="text-xl font-semibold mb-2">Mise en situation</h3>
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
              </div>
              {form.formState.errors.situation && (
                <span className="text-red-500">
                  {form.formState.errors.situation.message}
                </span>
              )}
            </div>
            <div className="border-b border-gray-700" />

            <div className="flex items-center justify-between">
              <h3 className="text-xl font-semibold mb-2">Code de base</h3>
              <Button className="border bg-gray-100 text-gray-900 cursor-pointer hover:bg-gray-200">
                python
              </Button>
            </div>
            <div className="h-96">
              <FormField
                name="code"
                control={form.control}
                render={({ field }) => (
                  <FormItem className="h-full">
                    <FormControl>
                      <CodeEditor
                        value={field.value ?? ""}
                        onChange={field.onChange}
                        placeholder="Code de base pour la situation..."
                      />
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>

            <div className="border-b border-gray-700" />

            <div className="flex justify-between items-center">
              <h3 className="text-xl font-semibold">Tests unitaires</h3>
              <Button className="bg-green-600 cursor-pointer hover:bg-green-700 text-white font-semibold py-2 px-4 rounded-lg">
                Ajouter un test
              </Button>
            </div>
            {/* {allTests.length > 0 && (
              <div className="space-y-6">
                {allTests.map((value, index) => (
                  <div
                    key={index}
                    className="bg-zinc-800 p-4 rounded-lg shadow-lg text-white"
                  >
                    <div className="flex items-center justify-between mb-4">
                      <h3 className="text-xl font-bold">Test {index + 1}</h3>
                      <div className="flex items-center space-x-4">
                        <Button
                          onClick={() => handleRemoveTest(index)}
                          className="text-red-400 cursor-pointer hover:text-red-600 transition"
                          title="Delete Test"
                        >
                          <Trash />
                        </Button>
                      </div>
                    </div>
                    <div className="items-top flex space-x-2 mb-4">
                      <Checkbox checked={value.isHidden} />
                      <div className="grid gap-1.5 leading-none">
                        <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
                          Cacher ce test aux étudiants
                        </label>
                        <p className="text-sm text-muted-foreground">
                          Les étudiants ne pourront pas voir ce test lors de
                          l'exécution de leur programme.
                        </p>
                      </div>
                    </div>
                    <div className="h-96">
                      <CodeEditor placeholder="Implémenter le test unitaire..." />
                    </div>
                  </div>
                ))}
              </div>
            )} */}
            <div className="border-b border-gray-700" />

            <div className="flex justify-end h-12">
              <Button
                type="submit"
                className="bg-green-600 cursor-pointer hover:bg-green-700 text-white font-semibold py-2 px-4 rounded-lg"
              >
                Soummettre
              </Button>
            </div>
          </form>
        </Form>
      </div>
    </AdminLayout>
  );
}
