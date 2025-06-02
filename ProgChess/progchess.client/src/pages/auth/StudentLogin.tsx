import { z } from "zod";
import { useState, useTransition } from "react";
import Layout from "../../components/layout/AuthLayout";
import AuthCard from "../../components/card/AuthCard";
import { Button } from "../../components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../components/ui/form";
import Flash from "../../components/flash/Flash";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Input } from "../../components/ui/input";
import { call } from "../../actions/auth";
import axios from "axios";

import { useNavigate, useParams } from "react-router";

const formSchema = z.object({
  code: z
    .string()
    .min(12, { message: "Code permanent invalide" })
    .max(12, { message: "Code permanent invalide" }),
  exerciceId: z.number(),
});

function StudentLogin() {
  const navigate = useNavigate();
  const [error, setError] = useState("");
  const [isPending, startTransition] = useTransition();

  const { id } = useParams();
  const decodedId = decodeURIComponent(id!);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      code: "",
      exerciceId: parseInt(decodedId.split("#")[1]) || 0,
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    startTransition(async () => {
      const response = await call(() =>
        axios.post("http://localhost:5290/api/auth/login-code", values, {
          withCredentials: true,
        })
      );
      form.reset();

      if (!response.success) {
        setError(response.error);
      } else {
        navigate(-1);
      }
    });
  }

  return (
    <Layout>
      <AuthCard>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <Flash type={"error"} message={error} />
            <FormField
              control={form.control}
              name="code"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Code Permanent</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="AAAA00000000"
                      className="text-white"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage className="text-red-600" />
                </FormItem>
              )}
            />
            <Button
              type="submit"
              className="w-full bg-green-500 cursor-pointer hover:bg-green-600"
              disabled={isPending}
            >
              {isPending ? "Connexion..." : "Connexion"}
            </Button>
          </form>
        </Form>
      </AuthCard>
    </Layout>
  );
}

export default StudentLogin;
