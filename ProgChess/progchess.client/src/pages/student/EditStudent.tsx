import { z } from "zod";
import AdminLayout from "../../components/layout/AdminLayout";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import api from "../../utils/api";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState, useTransition } from "react";
import type { StudentExercice } from "../../utils/type";
import { handleApiError } from "../../utils/apiErrorHandler";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import { toast } from "sonner";
import { Input } from "../../components/ui/input";
import { Button } from "../../components/ui/button";

const validationSchema = z.object({
  permanentCode: z
    .string()
    .min(12, { message: "Code permanent invalide" })
    .max(12, { message: "Code permanent invalide" })
    .regex(new RegExp(".*[a-zA-Z]{4}\\d{8}.*"), {
      message: "Code permanent invalide",
    }),
});

type formSchema = z.infer<typeof validationSchema>;

export default function EditStudent() {
  const [isPending, startTransition] = useTransition();
  const [studentExercise, setStudentExercise] = useState<StudentExercice>();
  const [oldPermanentCode, setOldPermanentCode] = useState<string>("");
  const navigate = useNavigate();
  const { id } = useParams();

  const form = useForm<formSchema>({
    resolver: zodResolver(validationSchema),
    defaultValues: {
      permanentCode: "",
    },
  });

  const getStudent = async () => {
    try {
      const response = await api.get(`/api/studentexercise/${id}`);
      setStudentExercise(response.data);
    } catch (error) {
      handleApiError(error);
    }
  };

  useEffect(() => {
    getStudent();
  }, []);

  useEffect(() => {
    if (studentExercise) {
      form.reset({
        permanentCode: studentExercise.studentPermanentCode,
      });
      setOldPermanentCode(studentExercise.studentPermanentCode);
    }
  }, [studentExercise]);

  const onSubmit = async (values: formSchema) => {
    startTransition(async () => {
      try {
        await api.put(`/api/studentexercise/edit/${id}`, {
          permanentCode: values.permanentCode,
          oldPermanentCode: oldPermanentCode,
        });
        toast.success("Étudiant modifié avec succès !");
        navigate("/admin/students");
      } catch (error) {
        handleApiError(error);
      }
    });
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col px-4 space-y-4 mt-4">
        <div className="flex">
          <h2 className="text-2xl font-bold text-white">
            Modifier un étudiant
          </h2>
        </div>
        <div className="border-b border-gray-700" />
        <Form {...form}>
          <form className="space-y-8" onSubmit={form.handleSubmit(onSubmit)}>
            <FormField
              control={form.control}
              name="permanentCode"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Code permanent</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="aaaa00000000"
                      className="text-white"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className="text-red-600" />
                </FormItem>
              )}
            />
            <div className="flex w-full justify-end">
              <Button
                type="submit"
                className="w-fit bg-green-500 cursor-pointer hover:bg-green-600"
                disabled={isPending}
              >
                {isPending ? "Soumission..." : "Soummettre"}
              </Button>
            </div>
          </form>
        </Form>
      </div>
    </AdminLayout>
  );
}
